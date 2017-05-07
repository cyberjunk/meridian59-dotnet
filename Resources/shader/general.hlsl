/********************************/
/* Vertex Shader Output Structs */
/********************************/

struct VOut1
{
	float4 p	: POSITION;
	float2 uv	: TEXCOORD0;
	float3 normal : TEXCOORD1;
};

struct VOut2
{
	float4 p    : POSITION;
	float2 uv   : TEXCOORD0;
	float4 wp   : TEXCOORD1;
};

struct VOut3
{
	float4 p		: POSITION;
	float2 uv		: TEXCOORD3;
	float2 uvnoise	: TEXCOORD0;
	float4 pproj	: TEXCOORD1;
};

struct VOut4 
{
	float4 p		: POSITION;
	float3 uvw		: TEXCOORD0;
	float3 normal	: TEXCOORD1;
	float3 vVec		: TEXCOORD2;
};

/********************************/
/*        VERTEX SHADERS        */
/********************************/

VOut1 ambient_vs(
	float4 p : POSITION,
	float2 uv : TEXCOORD0,
	float3 normal : NORMAL,
	uniform float4x4 wvpMat,
	uniform float4x4 texMat)
{
	VOut1 OUT;

	OUT.p  = mul(wvpMat, p);
	OUT.uv = mul(texMat, float4(uv, 0, 1)).xy;
	OUT.normal = normalize(normal);
	
	return OUT;
}

VOut2 diffuse_vs(
	float4 p : POSITION,
	float2 uv : TEXCOORD0,
	uniform float4x4 wvpMat,
	uniform float4x4 texMat,
	uniform float4x4 wMat)
{
	VOut2 OUT;

	OUT.p  = mul(wvpMat, p);
	OUT.uv = mul(texMat, float4(uv, 0, 1)).xy;
	OUT.wp = mul(wMat, p);

	return OUT;
}

VOut3 invisible_vs(
	float4 p : POSITION,
	float2 uv : TEXCOORD0,
	uniform float4x4 wvpMat,
	uniform float4x4 texMat,
	uniform float timeVal)
{
	VOut3 OUT;

	const float4x4 SCALEMAT = float4x4(
		0.5,  0.0, 0.0, 0.5,
		0.0, -0.5, 0.0, 0.5,
		0.0,  0.0, 0.5, 0.5,
		0.0,  0.0, 0.0, 1.0);

	OUT.p		= mul(wvpMat, p);
	OUT.uv		= mul(texMat, float4(uv, 0, 1)).xy;
	OUT.pproj	= mul(SCALEMAT, OUT.p);
	OUT.uvnoise = uv + timeVal;
	
	return OUT;
}

VOut4 water_vs(
	float4 p: POSITION,
	float3 normal : NORMAL,
	uniform float4x4 wvpMat,
	uniform float2 waveSpeed,
	uniform float time_0_X,
	uniform float3 eyePos)
{
	VOut4 OUT;

	const float NOISESPEED = 0.1;
	const float3 SCALE = float3(0.012, 0.005, 0.03);

	OUT.p      = mul(wvpMat, p);
	OUT.uvw	   = p.xyz * SCALE;
	OUT.uvw.xz += waveSpeed * time_0_X;
	OUT.uvw.y  += OUT.uvw.z + NOISESPEED * time_0_X;
	OUT.vVec   = p.xyz - eyePos;
	OUT.normal = normal;

	return OUT;
}

/********************************/
/*        PIXEL SHADERS         */
/********************************/

float4 ambient_ps(
	VOut1 vsout,
	uniform float3 ambient,
	uniform float3 lightCol,
	uniform float4 lightDir,
	uniform float4 colormodifier,
	uniform sampler2D diffusetex : TEXUNIT0) : COLOR0
{
	// pixel from texture
	float4 texcol = tex2D(diffusetex, vsout.uv);
	
	// flip direction
	lightDir = -lightDir;

	// represents how much this pixel should be affected by directional light
	float angle = max(dot(normalize(lightDir.xyz), vsout.normal), 0);
	
	// directional, ambient and combined light
	float3 dir  = angle * lightCol.rgb * texcol.rgb;
	float3 ambi = ambient.rgb * texcol.rgb;
	float3 sum  = (0.2 * dir) + (0.8 * ambi);
	
	// output pixel
	return float4(sum * colormodifier.rgb, texcol.a * colormodifier.a);
}

float4 diffuse_ps(
	VOut2 vsout,
	uniform float3 lightCol[8],
	uniform float4 lightPos[8],
	uniform float4 lightAtt[8],
	uniform float4 colormodifier,
	uniform sampler2D diffusetex : TEXUNIT0) : COLOR0
{  
	float lightScale;
	float3 light;
	float3 delta;
	
	// base pixel from texture
	float4 diffuseTex = tex2D(diffusetex, vsout.uv);
	
	// 1. light
	delta      = lightPos[0].xyz - vsout.wp.xyz;
	lightScale = 1.0 - (dot(delta, delta) / (lightAtt[0].r * lightAtt[0].r));
	light      = max(float3(0, 0, 0), lightCol[0] * lightScale);

	// 2. light
	delta      = lightPos[1].xyz - vsout.wp.xyz;
	lightScale = 1.0 - (dot(delta, delta) / (lightAtt[1].r * lightAtt[1].r));
	light      += max(float3(0, 0, 0), lightCol[1] * lightScale);

	// 3. light
	delta      = lightPos[2].xyz - vsout.wp.xyz;
	lightScale = 1.0 - (dot(delta, delta) / (lightAtt[2].r * lightAtt[2].r));
	light      += max(float3(0, 0, 0), lightCol[2] * lightScale);

	// 4. light
	delta      = lightPos[3].xyz - vsout.wp.xyz;
	lightScale = 1.0 - (dot(delta, delta) / (lightAtt[3].r * lightAtt[3].r));
	light      += max(float3(0, 0, 0), lightCol[3] * lightScale);

	// 5. light
	delta      = lightPos[4].xyz - vsout.wp.xyz;
	lightScale = 1.0 - (dot(delta, delta) / (lightAtt[4].r * lightAtt[4].r));
	light      += max(float3(0, 0, 0), lightCol[4] * lightScale);

	// 6. light
	delta      = lightPos[5].xyz - vsout.wp.xyz;
	lightScale = 1.0 - (dot(delta, delta) / (lightAtt[5].r * lightAtt[5].r));
	light      += max(float3(0, 0, 0), lightCol[5] * lightScale);

	// 7. light
	delta      = lightPos[6].xyz - vsout.wp.xyz;
	lightScale = 1.0 - (dot(delta, delta) / (lightAtt[6].r * lightAtt[6].r));
	light      += max(float3(0, 0, 0), lightCol[6] * lightScale);

	// 8. light
	delta      = lightPos[7].xyz - vsout.wp.xyz;
	lightScale = 1.0 - (dot(delta, delta) / (lightAtt[7].r * lightAtt[7].r));
	light      += max(float3(0, 0, 0), lightCol[7] * lightScale);

	return colormodifier * float4(diffuseTex.rgb * light.rgb, diffuseTex.a);
}

float4 invisible_ps(
	VOut3 vsout,
	uniform sampler2D diffusetex : TEXUNIT0,
	uniform sampler2D noisetex : TEXUNIT1,
	uniform sampler2D refracttex : TEXUNIT2) : COLOR0
{	
	float4 diffuseTex = tex2D(diffusetex, vsout.uv);
	
	if (diffuseTex.a > 0.0)
	{
		float3 noisen = (tex2D(noisetex, (vsout.uvnoise * 0.2)).rgb - 0.5).rbg * 0.05;
		float2 final  = (vsout.pproj.xy / vsout.pproj.w) + noisen.xz;

		return tex2D(refracttex, final);
	}	
	else
	{
		return diffuseTex;
	}
}

float4 water_ps(
	VOut4 vsout,
	uniform float3 ambient,
	uniform sampler2D noisetex : TEXUNIT0,
	uniform sampler2D diffusetex : TEXUNIT1) : COLOR0
{
	float3 noisy = tex2D(noisetex, vsout.uvw.xy).xyz;
	float3 bump = 2 * noisy - 1;
   
	bump.xz *= 0.15;
	bump.y = 0.8 * abs(bump.y) + 0.2;
	bump = normalize(vsout.normal + bump);

	float3 normView = normalize(vsout.vVec);
	float3 reflVec = reflect(normView, bump);
   
	reflVec.z = -reflVec.z;
   
	float4 reflcol = tex2D(diffusetex, reflVec.xy); 

	ambient = ambient + float3(0.01, 0.01, 0.01);
   
	return float4(ambient, 0) * reflcol;
}
