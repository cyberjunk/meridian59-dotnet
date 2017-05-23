
void SunVP(
   inout   float4   p  : POSITION,
   inout   float2   uv : TEXCOORD0,
   uniform float4x4 wvpMat,
   uniform float4x4 texMat)
 {
   p  = mul(wvpMat, p);
   uv = mul(texMat, float4(uv, 0, 1)).xy;
}

void SunFP(
   out     float4    pixel    : COLOR,
   in      float2    uv       : TEXCOORD0,
   uniform sampler2D sunDisc  : register(s0))
{
   // get texture pixel
   pixel = tex2D(sunDisc, uv);
}
