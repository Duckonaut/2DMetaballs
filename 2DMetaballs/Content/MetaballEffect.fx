const float4 black = float4(0.0, 0.0, 0.0, 0.0);
const float4 white = float4(1.0, 1.0, 1.0, 1.0);
const float edge = 0.00001f;

sampler2D SpriteTextureSampler;

Texture2D GalaxyTexture;

sampler2D GalaxySampler = sampler_state
{
    Texture = <GalaxyTexture>;
};

float width;
float height;
float2 offset;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float2 scaleBack(float2 pos)
{
    return float2(pos.x / width, pos.y / height);
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);

    float nearby[4]; // right then clockwise

    float2 pos = float2(input.TextureCoordinates.x * width, input.TextureCoordinates.y * height);

    nearby[0] = tex2D(SpriteTextureSampler, scaleBack(pos + float2(1, 0))).r;
    nearby[1] = tex2D(SpriteTextureSampler, scaleBack(pos + float2(0, 1))).r;
    nearby[2] = tex2D(SpriteTextureSampler, scaleBack(pos + float2(-1, 0))).r;
    nearby[3] = tex2D(SpriteTextureSampler, scaleBack(pos + float2(0, -1))).r;

    if (nearby[0] > edge && nearby[2] < edge)
    {
        return white;
    }
    else if (nearby[0] < edge && nearby[2] > edge)
    {
        return white;
    }
    else if (nearby[1] > edge && nearby[3] < edge)
    {
        return white;
    }
    else if (nearby[1] < edge && nearby[3] > edge)
    {
        return white;
    }

    if (color.r > edge)
    {
        return tex2D(GalaxySampler, input.TextureCoordinates + offset);
    }

    return black;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile ps_4_0 MainPS();
    }
};
