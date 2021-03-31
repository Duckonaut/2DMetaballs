const float4 black = float4(0.0, 0.0, 0.0, 0.0);
const float4 white = float4(1.0, 1.0, 1.0, 1.0);
const float4 codedColor = float4(0.0, 1.0, 0.0, 1.0);
const float edge = 0.4f;

sampler2D SpriteTextureSampler;

float width;
float height;

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

    if (nearby[0] > edge && color.r <= edge)
    {
        return white;
    }
    else if (nearby[1] > edge && color.r <= edge)
    {
        return white;
    }
    else if (nearby[2] > edge && color.r <= edge)
    {
        return white;
    }
    else if (nearby[3] > edge && color.r <= edge)
    {
        return white;
    }

    if (color.r > edge)
    {
        return codedColor;
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
