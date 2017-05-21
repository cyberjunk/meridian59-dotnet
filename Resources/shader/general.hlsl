/********************************/
/*     AMBIENT LIGHT SHADERS    */
/********************************/

// vertex
void ambient_vs(
   inout   float4   p      : POSITION,
   inout   float2   uv     : TEXCOORD0,
   inout   float3   normal : NORMAL,
   uniform float4x4 wvpMat,
   uniform float4x4 texMat)
{
   p = mul(wvpMat, p);
   uv = mul(texMat, float4(uv, 0, 1)).xy;
}

// pixel
void ambient_ps(
   out     float4    pixel          : COLOR0,
   in      float2    uv             : TEXCOORD0,
   in      float3    normal         : NORMAL,
   uniform float3    ambient,
   uniform float3    lightCol,
   uniform float4    lightDir,
   uniform float4    colormodifier,
   uniform sampler2D diffusetex     : TEXUNIT0)
{
   // pixel from texture
   const float4 texcol = tex2D(diffusetex, uv);

   // flip direction (ogre? also normaalize in ogre)
   lightDir = -lightDir;

   // represents how much this pixel should be affected by directional light
   float angle = max(dot(normalize(lightDir.xyz), normal), 0);

   // combine ambient and directional light with weights
   float3 light = (0.2 * angle * lightCol) + (0.8 * ambient);

   // output pixel
   pixel = float4(light * colormodifier.rgb * texcol.rgb, texcol.a * colormodifier.a);
}

/********************************/
/*       POINT LIGHT SHADERS    */
/********************************/

// vertex
void diffuse_vs(
   inout   float4   p  : POSITION,
   inout   float2   uv : TEXCOORD0,
   out     float4   wp : TEXCOORD1,
   uniform float4x4 wvpMat,
   uniform float4x4 texMat,
   uniform float4x4 wMat)
{
   wp = mul(wMat, p);
   p = mul(wvpMat, p);
   uv = mul(texMat, float4(uv, 0, 1)).xy;
}

// pixel
void diffuse_ps(
   out     float4    pixel         : COLOR0,
   in      float2    uv            : TEXCOORD0,
   in      float4    wp            : TEXCOORD1,
   uniform float3    lightCol[8],
   uniform float4    lightPos[8],
   uniform float4    lightAtt[8],
   uniform float4    colormodifier,
   uniform sampler2D diffusetex    : TEXUNIT0)
{
   float lightScale;
   float3 light;
   float3 delta;

   // base pixel from texture
   const float4 diffuseTex = tex2D(diffusetex, uv);

   // 1. light
   delta = lightPos[0] - wp;
   lightScale = max(0.0, 1.0 - (dot(delta, delta) / (lightAtt[0].r * lightAtt[0].r)));
   light = lightCol[0] * lightScale;

   // 2. light
   delta = lightPos[1] - wp;
   lightScale = max(0.0, 1.0 - (dot(delta, delta) / (lightAtt[1].r * lightAtt[1].r)));
   light += lightCol[1] * lightScale;

   // 3. light
   delta = lightPos[2] - wp;
   lightScale = max(0.0, 1.0 - (dot(delta, delta) / (lightAtt[2].r * lightAtt[2].r)));
   light += lightCol[2] * lightScale;

   // 4. light
   delta = lightPos[3] - wp;
   lightScale = max(0.0, 1.0 - (dot(delta, delta) / (lightAtt[3].r * lightAtt[3].r)));
   light += lightCol[3] * lightScale;

   // 5. light
   delta = lightPos[4] - wp;
   lightScale = max(0.0, 1.0 - (dot(delta, delta) / (lightAtt[4].r * lightAtt[4].r)));
   light += lightCol[4] * lightScale;

   // 6. light
   delta = lightPos[5] - wp;
   lightScale = max(0.0, 1.0 - (dot(delta, delta) / (lightAtt[5].r * lightAtt[5].r)));
   light += lightCol[5] * lightScale;

   // 7. light
   delta = lightPos[6] - wp;
   lightScale = max(0.0, 1.0 - (dot(delta, delta) / (lightAtt[6].r * lightAtt[6].r)));
   light += lightCol[6] * lightScale;

   // 8. light
   delta = lightPos[7] - wp;
   lightScale = max(0.0, 1.0 - (dot(delta, delta) / (lightAtt[7].r * lightAtt[7].r)));
   light += lightCol[7] * lightScale;

   pixel = colormodifier * float4(diffuseTex.rgb * light.rgb, diffuseTex.a);
}

/********************************/
/*       INVISIBLE SHADERS      */
/********************************/

// vertex
void invisible_vs(
   inout   float4   p       : POSITION,
   inout   float2   uv      : TEXCOORD0,
   out     float4   pproj   : TEXCOORD1,
   out     float2   uvnoise : TEXCOORD2,
   uniform float4x4 wvpMat,
   uniform float4x4 texMat,
   uniform float    timeVal)
{
   const float4x4 SCALEMAT = float4x4(
      0.5, 0.0, 0.0, 0.5,
      0.0, -0.5, 0.0, 0.5,
      0.0, 0.0, 0.5, 0.5,
      0.0, 0.0, 0.0, 1.0);

   p = mul(wvpMat, p);
   uvnoise = uv + timeVal;
   uv = mul(texMat, float4(uv, 0, 1)).xy;
   pproj = mul(SCALEMAT, p);
}

// pixel
void invisible_ps(
   out     float4    pixel      : COLOR0,
   in      float2    uv         : TEXCOORD0,
   in      float4    pproj      : TEXCOORD1,
   in      float2    uvnoise    : TEXCOORD2,
   uniform sampler2D diffusetex : TEXUNIT0,
   uniform sampler2D noisetex   : TEXUNIT1,
   uniform sampler2D refracttex : TEXUNIT2)
{
   // get color of pixel of object texture
   const float4 color = tex2D(diffusetex, uv);

   // skip anything half or more transparent
   clip(color.a - 0.5);

   // build randomized uv coords on refracttex
   float3 noisen = (tex2D(noisetex, (uvnoise * 0.2)).rgb - 0.5).rbg * 0.05;
   float2 final = (pproj.xy / pproj.w) + noisen.xz;

   pixel = tex2D(refracttex, final);
}

/********************************/
/*         WATER SHADERS        */
/********************************/

// vertex
void water_vs(
   inout   float4   p      : POSITION,
   inout   float3   normal : NORMAL,
   out     float3   uvw    : TEXCOORD0,
   out     float3   vVec   : TEXCOORD1,
   uniform float4x4 wvpMat,
   uniform float2   waveSpeed,
   uniform float    time_0_X,
   uniform float3   eyePos)
{
   const float NOISESPEED = 0.1;
   const float3 SCALE = float3(0.012, 0.005, 0.03);

   uvw = p.xyz * SCALE;
   uvw.xz += waveSpeed * time_0_X;
   uvw.y += uvw.z + NOISESPEED * time_0_X;
   vVec = p.xyz - eyePos;
   p = mul(wvpMat, p);
}

// pixel
void water_ps(
   out     float4    pixel      : COLOR0,
   in      float3    uvw        : TEXCOORD0,
   in      float3    vVec       : TEXCOORD1,
   in      float3    normal     : NORMAL,
   uniform float3    ambient,
   uniform sampler2D noisetex   : TEXUNIT0,
   uniform sampler2D diffusetex : TEXUNIT1)
{
   float3 noisy = tex2D(noisetex, uvw.xy).xyz;
   float3 bump = 2 * noisy - 1;

   bump.xz *= 0.15;
   bump.y = 0.8 * abs(bump.y) + 0.2;
   bump = normalize(normal + bump);

   float3 normView = normalize(vVec);
   float3 reflVec = reflect(normView, bump);
   float4 reflcol = tex2D(diffusetex, reflVec.xy);

   pixel = float4(ambient, 0) * reflcol;
}
