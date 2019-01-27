/********************************/
/*        NAME LABEL SHADERS    */
/********************************/

// vertex
void label_vs(
   inout   float4   p      : POSITION,
   inout   float2   uv     : TEXCOORD0,
   out     float4   wp     : TEXCOORD1,
   uniform float4x4 wMat,
   uniform float4x4 wvpMat,
   uniform float4x4 texMat,
   uniform float3   eyePos,
   uniform float    offset)
{
   // scale down the vertex position so the difference 
   // in distance from each vertex to camera is minimized
   p *= float4(0.001, 0.001, 0.001, 1);
   
   // now transform to world space and apply offset
   wp = mul(wMat, p);
   wp.y += offset;
   
   // scale in worldspace based on the distance to camera
   // bounds min and max sizes for label
   float scale = clamp(length(eyePos - wp), 10.0, 2000.0);
   
   // apply scale and offset
   p *= float4(scale, scale, scale, 1);   
   p.y += offset;   
   
   // final transform and create uv
   p = mul(wvpMat, p);
   uv = mul(texMat, float4(uv, 0, 1)).xy;
}

// pixel
void label_ps(
   out     float4    pixel          : COLOR0,
   in      float2    uv             : TEXCOORD0,
   uniform sampler2D diffusetex     : TEXUNIT0)
{
   pixel = tex2D(diffusetex, uv);
}

/********************************/
/*  QUEST MARKER LABEL SHADERS  */
/********************************/

// vertex
void questmarker_vs(
   inout   float4   p      : POSITION,
   inout   float2   uv     : TEXCOORD0,
   out     float4   wp     : TEXCOORD1,
   uniform float4x4 wMat,
   uniform float4x4 wvpMat,
   uniform float4x4 texMat,
   uniform float3   eyePos,
   uniform float    offset)
{
   // scale down the vertex position so the difference 
   // in distance from each vertex to camera is minimized
   p *= float4(0.001, 0.001, 0.001, 1);
   
   // now transform to world space and apply offset
   wp = mul(wMat, p);
   wp.y += offset;
   
   // scale in worldspace based on the distance to camera
   // bounds min and max sizes for label
   float scale = clamp(length(eyePos - wp), 10.0, 2000.0);
   
   // apply scale and offset
   p *= float4(scale, scale, scale, 1);   
   p.y += clamp((offset + (scale / 70.0)), 0.0, 300.0);

   // final transform and create uv
   p = mul(wvpMat, p);
   uv = mul(texMat, float4(uv, 0, 1)).xy;
}

/********************************/
/*     ROOM COMBINED SHADERS    */
/********************************/

// vertex
void room_vs(
   inout   float4   p      : POSITION,
   inout   float2   uv     : TEXCOORD0,
   out     float4   wp     : TEXCOORD1,
   inout   float3   normal : NORMAL,
   uniform float4x4 wMat,
   uniform float4x4 wvpMat,
   uniform float4x4 texMat)
{
   wp = mul(wMat, p);
   p = mul(wvpMat, p);
   uv = mul(texMat, float4(uv, 0, 1)).xy;
}

// pixel
void room_ps(
   out     float4    pixel          : COLOR0,
   in      float2    uv             : TEXCOORD0,
   in      float4    wp             : TEXCOORD1,
   in      float3    normal         : NORMAL,
   uniform float3    ambient,
   uniform float3    lightCol[48],
   uniform float4    lightPos[48],
   uniform float4    lightAtt[48],
   uniform sampler2D diffusetex     : TEXUNIT0)
{
   // pixel from texture
   const float4 texcol = tex2D(diffusetex, uv);

   // represents how much this pixel should be affected by directional light
   const float angle = max(dot(lightPos[0].xyz, normal), 0);

   // combine ambient and directional light with weights
   float3 light = (0.4 * angle * lightCol[0]) + (0.6 * ambient);

   [unroll(48)]
   for(uint i = 1; i < 48; i++)
   {
      const float3 delta = lightPos[i] - wp;
      const float lightScale = max(0.0, 1.0 - (dot(delta, delta) / (lightAtt[i].r * lightAtt[i].r)));
      light += lightCol[i] * lightScale;
   }
   
   // output pixel
   pixel = float4(light * texcol.rgb, texcol.a);
}

/********************************/
/*    OBJECT COMBINED SHADERS   */
/********************************/

// vertex
void object_vs(
   inout   float4   p      : POSITION,
   inout   float2   uv     : TEXCOORD0,
   out     float4   wp     : TEXCOORD1,
   inout   float3   normal : NORMAL,
   uniform float4x4 wMat,
   uniform float4x4 wvpMat,
   uniform float4x4 texMat,
   uniform float3   viewDir)
{
   wp = mul(wMat, p);
   p = mul(wvpMat, p);
   uv = mul(texMat, float4(uv, 0, 1)).xy;
   
   // for billboards the normals are not correct
   // they are always (0,0,0)
   // instead use the camera viewdirection with removed y component
   if (!any(normal))
   {
      normal = normalize(float3(viewDir.xz, 0));
      normal.x = -normal.x; 
   }
}

// pixel
void object_ps(
   out     float4    pixel          : COLOR0,
   in      float2    uv             : TEXCOORD0,
   in      float4    wp             : TEXCOORD1,
   in      float3    normal         : NORMAL,
   uniform float3    ambient,
   uniform float3    lightCol[8],
   uniform float4    lightPos[8],
   uniform float4    lightAtt[8],
   uniform float4    colormodifier,
   uniform float     sintime,
   uniform sampler2D diffusetex     : TEXUNIT0)
{
   // pixel from texture
   float4 texcol = tex2D(diffusetex, uv);

   // represents how much this pixel should be affected by directional light
   const float angle = max(dot(lightPos[0].xyz, normal), 0);

   // combine ambient and directional light with weights
   float3 light = (0.5 * angle * lightCol[0]) + (0.5 * ambient);

   [unroll(8)]
   for(uint i = 1; i < 8; i++)
   {
      const float3 delta = lightPos[i] - wp;
      const float lightScale = max(0.0, 1.0 - (dot(delta, delta) / (lightAtt[i].r * lightAtt[i].r)));
      light += lightCol[i] * lightScale;
   }
   
   // apply periodic brightness adjust (40-100%) over time (for some materials))
   texcol.rgb *= (0.4 + 0.6 * abs(sintime));
   
   // output pixel
   pixel = float4(light * colormodifier.rgb * texcol.rgb, texcol.a * colormodifier.a);
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
   
   // floor water
   if (normal.y != 0.0)
   {
      uvw.xz += waveSpeed * time_0_X;
      uvw.y += uvw.z + NOISESPEED * time_0_X;
   }
   
   // wall water
   else
   {
      uvw.y += -waveSpeed * time_0_X;
      uvw.xz += uvw.z + NOISESPEED * time_0_X;
   }
   
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
