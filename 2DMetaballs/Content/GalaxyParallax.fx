const float4 codedColor = float4(0.0, 1.0, 0.0, 1.0);

sampler2D SpriteTextureSampler;

Texture2D GalaxyTexture;

sampler2D GalaxySampler = sampler_state
{
    Texture = <GalaxyTexture>;
};

float screenWidth;
float screenHeight;
float width;
float height;
float2 offset;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);

    float2 coords = input.TextureCoordinates;
    coords.x *= screenWidth / width;
    coords.y *= screenHeight / height;



    offset.x /= width;
    offset.y /= height;

    if (color.r == codedColor.r && color.g == codedColor.g && color.b == codedColor.b && color.a == codedColor.a)
    {
        return tex2D(GalaxySampler, coords + offset);
    }

    return color;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile ps_4_0 MainPS();
    }
};
