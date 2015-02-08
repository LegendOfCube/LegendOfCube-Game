float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 NormalMatrix;

// A light position is is needed for diffuse and specular illumination
float3 ViewSpaceLightPosition;

// Defines the the ambient look of an object (simulate that all surfaces are somewhat lit)
float4 MaterialAmbientColor = float4(1.0, 1.0, 1.0, 1.0);
float MaterialAmbientIntensity = 0.1;

// Defines the the diffuse look of an object (light spread in all directions)
texture DiffuseTexture;
float4 MaterialDiffuseColor = float4(1.0, 1.0, 1.0, 1.0);
float MaterialDiffuseIntensity = 1.0;

// Defines the specular look of an object (highlights)
// An specular texture could be added
float4 MaterialSpecularColor = float4(1.0, 1.0, 1.0, 1.0);
float MaterialSpecularIntensity = 1.0;
float Shininess = 30.0;

// Defines the self illumination of an object
// An emmisive texture could be added
float4 MaterialEmissiveColor = float4(1.0, 1.0, 1.0, 1.0);
float MaterialEmissiveIntensity = 0.0;

sampler2D diffuseTextureSampler = sampler_state {
	Texture = (DiffuseTexture);
	mipfilter = linear;
	magfilter = linear;
	minfilter = anisotropic;
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
	// Interpolation can denormalize the normal, need to renormalize
	float3 normal = normalize(input.ViewSpaceNormal);

	// Let distance from light modify result. If a light from a sun was
	// simulated, this wouldn't be necessary.
	float3 lightDifference = ViewSpaceLightPosition - input.ViewSpacePos;
	float lightDistance = length(lightDifference);
	float lightDistanceFactor = 1.0 / (1 + pow(0.05 * lightDistance, 2));

	// Calculate some interesting direction vectors
	float3 directionToLight = normalize(lightDifference);
	float3 directionToEye = normalize(-input.ViewSpacePos);
	float3 h = normalize(directionToLight + directionToEye);

	// Sample the texture
	float4 diffuseTextureColor = tex2D(diffuseTextureSampler, input.TextureCoordinate);

	// Determine diffuse and specular effect (0 to 1)
	float diffuseFactor = lightDistanceFactor * max(0.0, dot(normal, directionToLight));
	float specularFactor = lightDistanceFactor * max(pow(dot(h, normal), Shininess), 0.0);

	// Determine final colors of different lighting component
	float4 ambient = MaterialAmbientIntensity * MaterialAmbientColor * diffuseTextureColor;
	float4 diffuse = diffuseFactor * MaterialDiffuseIntensity * MaterialDiffuseColor * diffuseTextureColor;
	float4 specular = specularFactor * MaterialSpecularIntensity * MaterialSpecularColor;
	float4 emissive = MaterialEmissiveIntensity * MaterialEmissiveColor;

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
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
