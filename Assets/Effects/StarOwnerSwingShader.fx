sampler uImage0 : register(s0); // 原本刀光
sampler uImage1 : register(s1); // 取色图
sampler uImage2 : register(s2); // 星辰

float4x4 uTransform;

struct VSInput
{
    float2 Pos : POSITION0;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

float4 SwingShader(PSInput input) : COLOR0
{
    float2 uv = input.Texcoord.xy;
    float4 color = tex2D(uImage0, uv);
    if(color.r == 0)
        return float4(0, 0, 0, 0);
    float4 colorMap = tex2D(uImage1, uv);
    color *= 3;
    color *= colorMap;
    if (color.r <= 0.1 && color.b <= 0.1 && color.g <= 0.1)
    {
        float4 stars = tex2D(uImage2, uv);
        color = stars;
    }
    return color;
}

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}
technique Technique1
{
    pass StarOwnerSwingShader
    {
        PixelShader = compile ps_2_0 SwingShader();
        VertexShader = compile vs_2_0 VertexShaderFunction();
    }
}