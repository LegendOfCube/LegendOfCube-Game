#define WHITE_COLOR float4(1.0, 1.0, 1.0, 1.0)
#define SHADOW_BIAS 0.0005f

// Will determine the bluriness of shadows, it's the distance between
// sampled shadow map coordinates in a 3x3 grid
#define PCF_SPACING 1.0 / 1200.0

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 NormalMatrix;

// Light position for diffuse and specular illumination
float PointLight0Strength = 20.0;
float3 PointLight0ViewSpacePos;
float4 PointLight0Color = WHITE_COLOR;
texture PointLight0ShadowMap;
float4x4 PointLight0ShadowMatrix;

// Defines the the ambient look of an object (simulate that all surfaces are somewhat lit)
float MaterialAmbientIntensity = 0.0;

// Defines the the diffuse look of an object (light spread in all directions)
bool UseDiffuseTexture;
texture DiffuseTexture;
float4 MaterialDiffuseColor = WHITE_COLOR;

// Defines the specular look of an object (highlights)
bool UseSpecularTexture;
texture SpecularTexture;
float4 MaterialSpecularColor = WHITE_COLOR;
float Shininess = 30.0;

// Defines the self illumination of an object
bool UseEmissiveTexture;
texture EmissiveTexture;
float4 MaterialEmissiveColor = WHITE_COLOR;

// Defines the normals for an object
texture NormalTexture;

// This sampler has been moved to the top as a way to fix a problem with the
// shadow map no longer being sampled with Point as filter, after the window
// has been resized. Why this actually happened is unresolved, this is not a
// proper fix.
sampler2D ShadowMapSampler = sampler_state {
	Texture = (PointLight0ShadowMap);
	MipFilter = none;
	MagFilter = Point;
	MinFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D diffuseTextureSampler = sampler_state {
	Texture = (DiffuseTexture);
	MipFilter = linear;
	MagFilter = linear;
	MinFilter = anisotropic;
	MaxAnisotropy = 16;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D specularTextureSampler = sampler_state {
	Texture = (SpecularTexture);
	MipFilter = linear;
	MagFilter = linear;
	MinFilter = anisotropic;
	MaxAnisotropy = 16;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D emissiveTextureSampler = sampler_state {
	Texture = (EmissiveTexture);
	MipFilter = linear;
	MagFilter = linear;
	MinFilter = anisotropic;
	MaxAnisotropy = 16;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D normalTextureSampler = sampler_state {
	Texture = (NormalTexture);
	MipFilter = linear;
	MagFilter = linear;
	MinFilter = anisotropic;
	MaxAnisotropy = 16;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float4 PointLight0ShadowMapCoord : TEXCOORD1;
	float3 ViewSpacePos : TEXCOORD2; // Using TEXCOORD looks incorrect, not sure if there is alternative
	float3 ViewSpaceNormal : TEXCOORD3;
};

// Separate structs with additional information needed for normal map calculation

struct NormalTexVertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float4 Tangent : TANGENT0;
	float4 Binormal : BINORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct NormalTexVertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float4 PointLight0ShadowMapCoord : TEXCOORD1;
	float3 ViewSpacePos : TEXCOORD2;
	float3 ViewSpaceNormal : TEXCOORD3;
	float3 ViewSpaceTangent : TEXCOORD4;
	float3 ViewSpaceBinormal : TEXCOORD5;
};

struct ShadowMapVertexShaderInput
{
	float4 Position : POSITION0;
};

struct ShadowMapVertexShaderOutput
{
	float4 Position : POSITION0;
	float4 ProjectPosition : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	// Perform space transforms
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	float4 projectPosition = mul(viewPosition, Projection);
	float4 viewSpaceNormal = normalize(mul(input.Normal, NormalMatrix));
	output.PointLight0ShadowMapCoord = mul(worldPosition, PointLight0ShadowMatrix);

	// Pass along to pixel shader
	output.Position = projectPosition;
	output.ViewSpacePos = viewPosition.xyz;
	output.ViewSpaceNormal = viewSpaceNormal.xyz;
	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

NormalTexVertexShaderOutput NormalTexVertexShaderFunction(NormalTexVertexShaderInput input)
{
	NormalTexVertexShaderOutput output;

	// Perform space transforms
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	float4 projectPosition = mul(viewPosition, Projection);
	float4 viewSpaceNormal = normalize(mul(input.Normal, NormalMatrix));
	output.PointLight0ShadowMapCoord = mul(worldPosition, PointLight0ShadowMatrix);

	// Pass along to pixel shader
	output.ViewSpacePos = viewPosition.xyz;
	output.Position = projectPosition;
	output.ViewSpaceNormal = viewSpaceNormal.xyz;
	output.TextureCoordinate = input.TextureCoordinate;

	output.ViewSpaceTangent = normalize(mul(input.Tangent, NormalMatrix).xyz);
	output.ViewSpaceBinormal = normalize(mul(input.Binormal, NormalMatrix).xyz);

	return output;
}

float4 MainPixelShading(float2 textureCoordinate, float4 lightSpacePos, float3 viewSpacePos, float3 normal)
{
	// Let distance from light modify result. If a light from a sun was
	// simulated, this wouldn't be necessary.
	float3 lightDifference = PointLight0ViewSpacePos - viewSpacePos;
	float lightDistance = length(lightDifference);
	float lightPreferenceFactor = 1.0 / PointLight0Strength;
	float lightDistanceFactor = 1.0 / (1.0 + pow(lightPreferenceFactor * lightDistance, 2.0));

	// Calculate some interesting direction vectors
	float3 directionToLight = normalize(lightDifference);
	float3 directionToEye = normalize(-viewSpacePos);
	float3 h = normalize(directionToLight + directionToEye);

	// Determine sample coord from light space position
	float2 shadowMapCoord = 0.5 * lightSpacePos.xy / lightSpacePos.w + float2(0.5, 0.5);
	shadowMapCoord.y = 1.0f - shadowMapCoord.y;

	// Default objects being lit, might want to do opposite if scene is dark
	float visibility = 1.0;

	// Sample shadow map if inside
	// (setting border color on sampler seems to be deprecated)
	float shadowLookupMin = PCF_SPACING;
	float shadowLookupMax = 1.0 - PCF_SPACING;

	if (shadowMapCoord.x >= shadowLookupMin && shadowMapCoord.x <= shadowLookupMax &&
	    shadowMapCoord.y >= shadowLookupMin && shadowMapCoord.y <= shadowLookupMax)
	{
		visibility = 0.0;
		// Sample 9 points around actual coordinate, 3x3 grid
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				float2 offset = PCF_SPACING * float2(x, y);
				float shadowMapDepth = tex2D(ShadowMapSampler, shadowMapCoord + offset).x;
				float lightSpaceDepth = lightSpacePos.z / lightSpacePos.w;
				float sampleContribution = 1.0 / 9.0;
				visibility += ((shadowMapDepth + SHADOW_BIAS) > lightSpaceDepth) ? sampleContribution : 0.0;
			}
		}
	}

	float4 diffuseTextureColor = UseDiffuseTexture ? tex2D(diffuseTextureSampler, textureCoordinate) : WHITE_COLOR;
	float4 specularTextureColor = UseSpecularTexture ? tex2D(specularTextureSampler, textureCoordinate) : WHITE_COLOR;
	float4 emissiveTextureColor = UseEmissiveTexture ? tex2D(emissiveTextureSampler, textureCoordinate) : WHITE_COLOR;

	// Determine diffuse, specular and fresnel factor (0 to 1)
	float diffuseFactor = lightDistanceFactor * max(0.0, dot(normal, directionToLight));
	float specularDot = dot(h, normal);
	float specularFactor = specularDot <= 0.0 ? 0.0 : lightDistanceFactor * max(pow(specularDot, Shininess), 0.0);
	float fresnelFactor = pow(clamp(1.0 - dot(directionToEye, normal), 0.0, 1.0), 5.0);

	// Apply fresnel effect
	float4 specularCombined = MaterialSpecularColor * specularTextureColor;
	float4 fresnelSpecular = specularCombined + (WHITE_COLOR - specularCombined) * fresnelFactor;

	// Determine final colors of different lighting component
	float4 ambient = MaterialAmbientIntensity * MaterialDiffuseColor * diffuseTextureColor; // Currently locked to diffuse color
	float4 diffuse = visibility * diffuseFactor * PointLight0Color * MaterialDiffuseColor * diffuseTextureColor;
	float4 specular = visibility * specularFactor * PointLight0Color * fresnelSpecular;
	float4 emissive = MaterialEmissiveColor * emissiveTextureColor;

	//return float4(visibility.xxx, 1.0);
	return saturate(
		ambient +
		diffuse +
		specular +
		emissive
	);
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Interpolation can denormalize the normal, need to renormalize
	float3 normal = normalize(input.ViewSpaceNormal);
	return MainPixelShading(input.TextureCoordinate, input.PointLight0ShadowMapCoord, input.ViewSpacePos, normal);
}

float4 NormalTexPixelShaderFunction(NormalTexVertexShaderOutput input) : COLOR0
{
	// Get normal from normal map
	float4 normalTexVec = 2.0 * tex2D(normalTextureSampler, input.TextureCoordinate) - 1.0;

	// Inverting green channel might be needed when exporing from some 3D programs
	// normalTexVec.y = -normalTexVec.y;

	float3 normal = normalize(
		normalTexVec.x * normalize(input.ViewSpaceTangent) +
		normalTexVec.y * normalize(input.ViewSpaceBinormal) +
		normalTexVec.z * normalize(input.ViewSpaceNormal)
	);

	return MainPixelShading(input.TextureCoordinate, input.PointLight0ShadowMapCoord, input.ViewSpacePos, normal);
}

ShadowMapVertexShaderOutput ShadowMapVertexShaderFunction(ShadowMapVertexShaderInput input)
{
	ShadowMapVertexShaderOutput output;

	// Perform space transforms
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	float4 projectPosition = mul(viewPosition, Projection);

	// Pass along to pixel shader
	output.Position = projectPosition;
	output.ProjectPosition = projectPosition;

	return output;
}

float4 ShadowMapPixelShaderFunction(ShadowMapVertexShaderOutput input) : COLOR0
{
	float depth = input.ProjectPosition.z / input.ProjectPosition.w;
	return float4(depth.xxx, 1.0);
}

technique DefaultTechnique
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}

technique NormalMapTechnique
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 NormalTexVertexShaderFunction();
		PixelShader = compile ps_3_0 NormalTexPixelShaderFunction();
	}
}

technique ShadowMapTechnique
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 ShadowMapVertexShaderFunction();
		PixelShader = compile ps_3_0 ShadowMapPixelShaderFunction();
	}
}
