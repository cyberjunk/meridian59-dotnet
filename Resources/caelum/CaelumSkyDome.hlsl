/*
This file is part of Caelum.
See http://www.ogre3d.org/wiki/index.php/Caelum 

Copyright (c) 2006-2007 Caelum team. See Contributors.txt for details.

Caelum is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Caelum is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with Caelum. If not, see <http://www.gnu.org/licenses/>.
*/

float bias (in float b, in float x)
{
   return pow(x, log(b) / log(0.5));
}

float fogExp(in float z, in float density)
{
   return 1 - clamp(pow(2.71828, -z * density), 0, 1);
}

float4 sunlightInscatter(
   in float4 sunColour,
   in float  absorption,
   in float  incidenceAngleCos,
   in float  sunlightScatteringFactor)
{
   float scatteredSunlight = bias(sunlightScatteringFactor * 0.5, incidenceAngleCos);
   sunColour = sunColour * (1 - absorption) * float4 (0.9, 0.5, 0.09, 1);
   return sunColour * scatteredSunlight;
}

void SkyDomeVP(
   inout   float4   position          : POSITION,
   inout   float3   normal            : NORMAL,
   inout   float2   uv                : TEXCOORD0,
   out     float    incidenceAngleCos : TEXCOORD1,
   out     float    y                 : TEXCOORD2,
   uniform float    lightAbsorption,
   uniform float4x4 worldViewProj,
   uniform float3   sunDirection)
{
   sunDirection = -normalize(sunDirection);
   normal       = normalize(normal);

   incidenceAngleCos = -dot(sunDirection, normal);
   y = sunDirection.y;

   position = mul(worldViewProj, position);
   normal   = -normal;
}

void SkyDomeFP(
   in      float2    uv                : TEXCOORD0,
   in      float     incidenceAngleCos : TEXCOORD1,
   in      float     y                 : TEXCOORD2, 
   in      float3    normal            : NORMAL, 
   uniform sampler   gradientsMap      : register(s0), 
   uniform sampler1D atmRelativeDepth  : register(s1), 
   uniform float4    hazeColour, 
   uniform float     offset,
   out     float4    oCol : COLOR)
{
   float4 sunColour = float4 (3, 3, 3, 1);

#ifdef HAZE
   const float fogDensity = 15;
   const float invHazeHeight = 100;
   const float haze = fogExp (pow(clamp (1 - normal.y, 0, 1), invHazeHeight), fogDensity);
#endif // HAZE

   // Pass the colour
   oCol = tex2D (gradientsMap, uv + float2 (offset, 0));

   // Sunlight inscatter
   if (incidenceAngleCos > 0)
   {
      float sunlightScatteringFactor = 0.01;
      float sunlightScatteringLossFactor = 0.7;
      float atmLightAbsorptionFactor = 0.5;

      oCol.rgb += sunlightInscatter(
         sunColour, 
         clamp (atmLightAbsorptionFactor * (1 - tex1D (atmRelativeDepth, y).r), 0, 1), 
         clamp (incidenceAngleCos, 0, 1), 
         sunlightScatteringFactor).rgb * (1 - sunlightScatteringLossFactor);
   }

#ifdef HAZE
   // Haze pass
   hazeColour.a = 1;
   oCol = oCol * (1 - haze) + hazeColour * haze;
#endif // HAZE
}
