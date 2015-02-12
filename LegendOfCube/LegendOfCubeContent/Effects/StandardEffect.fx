#define WHITE_COLOR float4(1.0, 1.0, 1.0, 1.0)

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 NormalMatrix;

// Light position for diffuse and specular illumination
float PointLight0Strength = 20.0;
float3 PointLight0ViewSpacePos;
float4 PointLight0Color = WHITE_COLOR;

// Defines the the ambient look of an object (simulate that all surfaces are somewhat lit)
float4 MaterialAmbientIntensity = 1.0;

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
bool UseNormalTexture;
texture NormalTexture;

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
	float3 ViewSpacePos : TEXCOORD1; // Using TEXCOORD looks incorrect, not sure if there is alternative
	float3 ViewSpaceNormal : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	// Perform space transforms
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	float4 projecPosition = mul(viewPosition, Projection);
	float4 viewSpaceNormal = normalize(mul(input.Normal, NormalMatrix));

	// Pass along to pixel shader
	output.ViewSpacePos = viewPosition;
	output.Position = projecPosition;
	output.ViewSpaceNormal = viewSpaceNormal;
	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	
	float3 normal;
	if (UseNormalTexture)
	{
		// Get normal from normal map
		float4 modelNormal = 2.0 * tex2D(normalTextureSampler, input.TextureCoordinate) - 1.0;
		float4 normal4 = mul(NormalMatrix, modelNormal);
		normal = normalize(modelNormal.xyz);
	}
	else
	{
		// Interpolation can denormalize the normal, need to renormalize
		normal = normalize(input.ViewSpaceNormal);
	}

	// Let distance from light modify result. If a light from a sun was
	// simulated, this wouldn't be necessary.
	float3 lightDifference = PointLight0ViewSpacePos - input.ViewSpacePos;
	float lightDistance = length(lightDifference);
	float lightPreferenceFactor = 1.0 / PointLight0Strength;
	float lightDistanceFactor = 1.0 / (1.0 + pow(lightPreferenceFactor * lightDistance, 2.0));

	// Calculate some interesting direction vectors
	float3 directionToLight = normalize(lightDifference);
	float3 directionToEye = normalize(-input.ViewSpacePos);
	float3 h = normalize(directionToLight + directionToEye);

	float4 diffuseTextureColor = UseDiffuseTexture ? tex2D(diffuseTextureSampler, input.TextureCoordinate) : WHITE_COLOR;
	float4 specularTextureColor = UseSpecularTexture ? tex2D(specularTextureSampler, input.TextureCoordinate) : WHITE_COLOR;
	float4 emissiveTextureColor = UseEmissiveTexture ? tex2D(emissiveTextureSampler, input.TextureCoordinate) : WHITE_COLOR;

	// Determine diffuse and specular effect (0 to 1)
	float diffuseFactor = lightDistanceFactor * max(0.0, dot(normal, directionToLight));
	float specularFactor = lightDistanceFactor * max(pow(dot(h, normal), Shininess), 0.0);

	// Determine final colors of different lighting component
	float4 ambient = MaterialAmbientIntensity * MaterialDiffuseColor * diffuseTextureColor; // Right now locked to diffuse color
	float4 diffuse = diffuseFactor * PointLight0Color * MaterialDiffuseColor * diffuseTextureColor;
	float4 specular = specularFactor * PointLight0Color * MaterialSpecularColor * specularTextureColor;
	float4 emissive =  MaterialEmissiveColor * emissiveTextureColor;

	return saturate(
		ambient +
		diffuse +
		specular +
		emissive
	);

}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}
