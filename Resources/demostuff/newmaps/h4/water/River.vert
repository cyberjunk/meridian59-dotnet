// River shader by Martin Wissmiller, based on the River shader by Frans van Hoesel


struct VS_OUTPUT {
	float4 Pos: POSITION;
	float2 texNormal0Coord: TEXCOORD0;
	float2 texColorCoord: TEXCOORD1;
	float2 texFlowCoord: TEXCOORD2;
	float3 Normal: TEXCOORD3;
	float3 EyeDir: TEXCOORD4;
	float myTime: TEXCOORD5;
};

VS_OUTPUT main (
	float4 Pos: POSITION, 
	float3 normal: NORMAL,
	uniform float4x4 worldViewProj,
	uniform float3 eyePosition,
	uniform float time_0_X,
	float2 texNormal0Coord: TEXCOORD0,
	float2 texFlowCoord: TEXCOORD2,
	float2 texColorCoord: TEXCOORD3
)
{
	VS_OUTPUT Out;
	
	Out.Pos = mul(worldViewProj, Pos);	
	Out.Normal = normal;	
    Out.EyeDir = Pos.xyz - eyePosition;
	Out.myTime = 0.01 * time_0_X;
	
	Out.texNormal0Coord   = texNormal0Coord.xy;
    Out.texColorCoord = texColorCoord.xy;
    Out.texFlowCoord = texFlowCoord.xy;
	
	return Out;
}
	
	




