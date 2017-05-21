uniform sampler2D RT : register(s0);

// vertex
void Compositor_vs(
   inout   float4   p  : POSITION,
   inout   float2   uv : TEXCOORD0,
   uniform float4x4 cWorldViewProj)
{
   p = mul(cWorldViewProj, p);
}

// examplecompositor
float4 BlackAndWhite_ps(float2 iTexCoord : TEXCOORD0) : COLOR
{
   float3 greyscale = dot(tex2D(RT, iTexCoord).rgb, float3(0.3, 0.59, 0.11));
   return float4(greyscale, 1.0);
}

// blend (used by blind, whiteout, pain and others)
void Blend_ps(
   out     float4 pixel : COLOR0,
   in      float2 uv    : TEXCOORD0,
   uniform float4 blendcolor)
{
   // get pixelcolor
   const float4 color = tex2D(RT, uv);

   // get the weight of the base-color
   const float oneminusa = 1.0 - blendcolor.a;

   // apply the weights on base and blend color
   // and combine them
   const float4 blend = blendcolor.a * blendcolor;
   const float4 base  = oneminusa * color;

   pixel = blend + base;
}

// invert (used by bonk)
void Invert_ps(
   out float4 pixel : COLOR0,
   in  float2 uv    : TEXCOORD0)
{
   // get pixelcolor and invert
   pixel = float4(1, 1, 1, 1) - float4(tex2D(RT, uv).rgb, 0);
}

// blur (used for alcohol)
void Blur_ps(
   out     float4 pixel : COLOR0,
   in      float2 uv    : TEXCOORD0,
   uniform float  sampleDist,
   uniform float  sampleStrength)
{
   // save the color to be used later
   float4 color = tex2D(RT, uv);

   // vector from pixel to the center of the screen
   float2 dir = 0.5 - uv;

   // distance from pixel to the center (distant pixels have stronger effect)
   float dist = length(dir);

   // Calculate amount of blur based on
   // distance and a strength parameter
   // We need 0 <= t <= 1
   float t = saturate(dist * sampleStrength);

   // now that we have dist, we can normlize vector
   dir = normalize(dir);

   // uv modifications for pixels in the neighbourhood
   const float samples[10] =
   {
      -0.08, -0.05, -0.03, -0.02, -0.01,
       0.01,  0.02,  0.03,  0.05,  0.08
   };

   // average the pixels going along the vector
   float4 sum = color;

   [unroll(10)]
   for (int i = 0; i < 10; i++)
   {
      sum += tex2D(RT, uv + dir * samples[i] * sampleDist);
   }
   sum /= 11.0;

   // blend the original color with the averaged pixels
   pixel = lerp(color, sum, t);
}
