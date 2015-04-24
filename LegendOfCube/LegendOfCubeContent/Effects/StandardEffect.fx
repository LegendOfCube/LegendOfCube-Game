#define WHITE_COLOR float4(1.0, 1.0, 1.0, 1.0)

// Will determine the bluriness of shadows, it's the distance between
// sampled shadow map coordinates in a 3x3 grid
#define LEVEL_0_PCF_SPACING 1.0 / 2000.0
#define LEVEL_1_PCF_SPACING 1.0 / 3000.0

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 NormalMatrix;

// Light properties for diffuse and specular illumination
float3 DirLight0ViewSpaceDir;
float4 DirLight0Color = WHITE_COLOR;
bool ApplyShadows;
float4x4 DirLight0ShadowMatrix0;
float4x4 DirLight0ShadowMatrix1;

float3 PointLight0ViewSpacePos;
float4 PointLight0Color = WHITE_COLOR;
float PointLight0Reach = 20.0;

// Defines the the ambient look of an object (simulate that all surfaces are somewhat lit)
float AmbientIntensity = 0.0;

// Defines the the diffuse look of an object (light spread in all directions)
bool UseDiffuseTexture;
float4 MaterialDiffuseColor = WHITE_COLOR;

// Defines the specular look of an object (highlights)
bool UseSpecularTexture;
float4 MaterialSpecularColor = WHITE_COLOR;
float Shininess = 30.0;

// Defines the self illumination of an object
bool UseEmissiveTexture;
float4 MaterialEmissiveColor = WHITE_COLOR;

// These are set at XNA-level by using I from sI as an index
sampler DiffuseTextureSampler  : register(ps, s0);
sampler SpecularTextureSampler : register(ps, s1);
sampler EmissiveTextureSampler : register(ps, s2);
sampler NormalTextureSampler   : register(ps, s3);
sampler ShadowMapSampler0      : register(ps, s4);
sampler ShadowMapSampler1      : register(ps, s5);

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
	float4 DirLight0ShadowMapCoord0 : TEXCOORD1;
	float4 DirLight0ShadowMapCoord1 : TEXCOORD2;
	float3 ViewSpacePos : TEXCOORD3; // Using TEXCOORD looks incorrect, not sure if there is alternative
	float3 ViewSpaceNormal : TEXCOORD4;
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
	float4 DirLight0ShadowMapCoord0 : TEXCOORD1;
	float4 DirLight0ShadowMapCoord1 : TEXCOORD2;
	float3 ViewSpacePos : TEXCOORD3;
	float3 ViewSpaceNormal : TEXCOORD4;
	float3 ViewSpaceTangent : TEXCOORD5;
	float3 ViewSpaceBinormal : TEXCOORD6;
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
	output.DirLight0ShadowMapCoord0 = mul(worldPosition, DirLight0ShadowMatrix0);
	output.DirLight0ShadowMapCoord1 = mul(worldPosition, DirLight0ShadowMatrix1);

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
	output.DirLight0ShadowMapCoord0 = mul(worldPosition, DirLight0ShadowMatrix0);
	output.DirLight0ShadowMapCoord1 = mul(worldPosition, DirLight0ShadowMatrix1);

	// Pass along to pixel shader
	output.ViewSpacePos = viewPosition.xyz;
	output.Position = projectPosition;
	output.ViewSpaceNormal = viewSpaceNormal.xyz;
	output.TextureCoordinate = input.TextureCoordinate;

	output.ViewSpaceTangent = normalize(mul(input.Tangent, NormalMatrix).xyz);
	output.ViewSpaceBinormal = normalize(mul(input.Binormal, NormalMatrix).xyz);

	return output;
}

bool CoveredByShadowMap(float2 shadowMapCoord, float pcfSpacing)
{
	float shadowLookupMin = pcfSpacing;
	float shadowLookupMax = 1.0 - pcfSpacing;
	return shadowMapCoord.x >= shadowLookupMin && shadowMapCoord.x <= shadowLookupMax &&
		shadowMapCoord.y >= shadowLookupMin && shadowMapCoord.y <= shadowLookupMax;
}

float SampleShadowMapPCF(sampler ShadowMapSampler, float2 shadowMapCoord, float4 lightSpacePos, float pcfSpacing)
{
	float visibility = 0.0;
	for (int x = -1; x <= 1; x++)
	{
		for (int y = -1; y <= 1; y++)
		{
			float2 offset = pcfSpacing * float2(x, y);
			float shadowMapDepth = tex2D(ShadowMapSampler, shadowMapCoord + offset).x;
			float lightSpaceDepth = lightSpacePos.z / lightSpacePos.w;
			float sampleContribution = 1.0 / 9.0;
			visibility += (shadowMapDepth > lightSpaceDepth) ? sampleContribution : 0.0;
		}
	}
	return visibility;
}

// General light constribution function, used by different types of lights
float4 CalculateLightContribution(
	float3 normal,
	float3 directionToLight,
	float3 directionToEye,
	float4 lightColor,
	float4 diffuseColor,
	float4 specularColor)
{
	float3 h = normalize(directionToLight + directionToEye);
	float specularDot = dot(h, normal);

	// Determine diffuse, specular and fresnel factor (0 to 1)
	// At the moment, scale light with negative ambient intensity to easier be
	// able to balance light for different levels
	float diffuseFactor = (1.0 - AmbientIntensity) * max(0.0, dot(normal, directionToLight));
	float specularFactor = (1.0 - AmbientIntensity) * specularDot <= 0.0 ? 0.0 : max(pow(specularDot, Shininess), 0.0);
	float fresnelFactor = pow(clamp(1.0 - dot(directionToEye, normal), 0.0, 1.0), 5.0);

	// Apply fresnel effect
	float4 fresnelSpecular = specularColor + (WHITE_COLOR - specularColor) * fresnelFactor;

	float4 diffuse = diffuseFactor * lightColor * diffuseColor;
	float4 specular = specularFactor * lightColor * fresnelSpecular;
	return diffuse + specular;
}

// Calculate the contribution of a directional light
// TODO: Group properties by structs to make it more readable
float4 CalculateDirLightContribution(
	float4 lightSpacePos0,
	float4 lightSpacePos1,
	float3 lightDir,
	float4x4 shadowMatrix,
	float3 normal,
	float3 directionToEye,
	float4 lightColor,
	float4 diffuseColor,
	float4 specularColor)
{
	// Determine sample coord from light space position
	float2 shadowMapCoord0 = 0.5 * lightSpacePos0.xy / lightSpacePos0.w + float2(0.5, 0.5);
	shadowMapCoord0.y = 1.0 - shadowMapCoord0.y;

	// Default to objects being lit, might want to do opposite if scene is dark
	float visibility = 1.0;
	if (ApplyShadows) {

		// Sample shadow map if inside
		// (setting border color on sampler seems to be deprecated)
		if (CoveredByShadowMap(shadowMapCoord0, LEVEL_0_PCF_SPACING))
		{
			// Sample 9 points around actual coordinate, 3x3 grid
			visibility = SampleShadowMapPCF(ShadowMapSampler0, shadowMapCoord0, lightSpacePos0, LEVEL_0_PCF_SPACING);
		}
		else
		{
			// Check if fragment is covered by the less detailed shadow map
			float2 shadowMapCoord1 = 0.5 * lightSpacePos1.xy / lightSpacePos1.w + float2(0.5, 0.5);
			shadowMapCoord1.y = 1.0 - shadowMapCoord1.y;

			if (CoveredByShadowMap(shadowMapCoord1, LEVEL_1_PCF_SPACING))
			{
				visibility = SampleShadowMapPCF(ShadowMapSampler1, shadowMapCoord1, lightSpacePos1, LEVEL_1_PCF_SPACING);
			}
		}

	}
	return visibility * CalculateLightContribution(normal, normalize(-lightDir), directionToEye, lightColor, diffuseColor, specularColor);
}

// Calculate the contribution of a point light
// TODO: Group properties by structs to make it more readable
float4 CalculatePointLightContribution(
	float3 viewSpacePos,
	float3 lightPosition,
	float lightReach,
	float3 normal,
	float3 directionToEye,
	float4 lightColor,
	float4 diffuseColor,
	float4 specularColor)
{
	// Let distance from light modify result
	float3 lightDifference = lightPosition - viewSpacePos;
	float lightDistance = length(lightDifference);
	float lightInverseFactor = 1.0 / (1.0 + lightReach);
	float lightDistanceFactor = saturate(1.0 / (1.0 + lightInverseFactor * lightDistance * lightDistance));

	return lightDistanceFactor * CalculateLightContribution(normal, normalize(lightDifference), directionToEye, lightColor, diffuseColor, specularColor);
}

float4 MainPixelShading(float2 textureCoordinate, float4 lightSpacePos0, float4 lightSpacePos1, float3 viewSpacePos, float3 normal)
{
	// Determine material color, from texture if available
	float4 diffuseColor = MaterialDiffuseColor;
	if (UseDiffuseTexture)
	{
		diffuseColor *= tex2D(DiffuseTextureSampler, textureCoordinate);
	}
	float4 specularColor = MaterialSpecularColor;
	if (UseSpecularTexture)
	{
		specularColor *= tex2D(SpecularTextureSampler, textureCoordinate);
	}
	float4 emissiveColor = MaterialEmissiveColor;
	if (UseEmissiveTexture)
	{
		emissiveColor *= tex2D(EmissiveTextureSampler, textureCoordinate);
	}

	float3 directionToEye = normalize(-viewSpacePos);
	// Calculate light contribution
	float4 dirLight0Contribution = CalculateDirLightContribution(lightSpacePos0, lightSpacePos1, DirLight0ViewSpaceDir, DirLight0ShadowMatrix0, normal, directionToEye, DirLight0Color, diffuseColor, specularColor);
	float4 pointLight0Contribution = CalculatePointLightContribution(viewSpacePos, PointLight0ViewSpacePos, PointLight0Reach, normal, directionToEye, PointLight0Color, diffuseColor, specularColor);

	float4 totalLightContribution = dirLight0Contribution + pointLight0Contribution;

	float4 ambientFinal = AmbientIntensity * diffuseColor; // Currently locked to diffuse color
	float4 emissiveFinal = emissiveColor;

	return saturate(
		ambientFinal +
		totalLightContribution +
		emissiveFinal
	);
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Interpolation can denormalize the normal, need to renormalize
	float3 normal = normalize(input.ViewSpaceNormal);
	return MainPixelShading(input.TextureCoordinate, input.DirLight0ShadowMapCoord0, input.DirLight0ShadowMapCoord1, input.ViewSpacePos, normal);
}

float4 NormalTexPixelShaderFunction(NormalTexVertexShaderOutput input) : COLOR0
{
	// Get normal from normal map
	float4 normalTexVec = 2.0 * tex2D(NormalTextureSampler, input.TextureCoordinate) - 1.0;

	// Inverting green channel might be needed when exporing from some 3D programs
	// normalTexVec.y = -normalTexVec.y;

	float3 normal = normalize(
		normalTexVec.x * normalize(input.ViewSpaceTangent) +
		normalTexVec.y * normalize(input.ViewSpaceBinormal) +
		normalTexVec.z * normalize(input.ViewSpaceNormal)
	);

	return MainPixelShading(input.TextureCoordinate, input.DirLight0ShadowMapCoord0, input.DirLight0ShadowMapCoord1, input.ViewSpacePos, normal);
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
