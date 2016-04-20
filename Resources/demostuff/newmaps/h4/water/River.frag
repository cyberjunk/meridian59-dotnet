// River shader by Martin Wissmiller, based on the River shader by Frans van Hoesel

float4 main (
	float2 texNormal0Coord: TEXCOORD0,
	float2 texColorCoord: TEXCOORD1,
	float2 texFlowCoord: TEXCOORD2,
	float3 Normal: TEXCOORD3,
	float3 EyeDir: TEXCOORD4,
	float myTime: TEXCOORD5,
	
	uniform sampler2D normalMap,
	uniform sampler2D flowMap,
	uniform sampler2D colorMap,
	uniform samplerCUBE cubeMap,
	uniform samplerCUBE cubeMap2
	
) : COLOR

{
	
    float texScale = 105.0;
    float texScale2 = 30.0; 
    float myangle;
    float transp;
    float3 myNormal;
	
    float2 mytexFlowCoord = texFlowCoord * texScale;
     
    float2 ff =  abs(2.0*(frac(mytexFlowCoord)) - 1.0) -0.5;      

    ff = 0.5 - 4.0*ff*ff*ff;

    float2 ffscale = sqrt(ff*ff + (1.0-ff)*(1.0-ff));
	
    float2 Tcoord = texNormal0Coord  * texScale2;
    //myTime = myTime * tex2D(speedMap, texFlowCoord).rgb;
    float2 offset = float2(myTime,0);
    
    float3 sample = tex2D( flowMap, floor(mytexFlowCoord)/ texScale).rgb;     
    float2 flowdir = sample.xy -0.5;      
    flowdir *= sample.b;  
    float2x2 rotmat = float2x2(flowdir.x, -flowdir.y, flowdir.y ,flowdir.x);
	
    float2 NormalT0 = tex2D(normalMap, mul(Tcoord, rotmat) - offset).rg;
    
    sample = tex2D( flowMap, floor((mytexFlowCoord + float2(0.5,0)))/ texScale ).rgb;
    flowdir = sample.b * (sample.xy - 0.5);
    rotmat = float2x2(flowdir.x, -flowdir.y, flowdir.y ,flowdir.x);

    float2 NormalT1 = tex2D(normalMap, mul(Tcoord, rotmat) - offset*1.06+0.62).rg ;    
    float2 NormalTAB = ff.x * NormalT0 + (1.0-ff.x) * NormalT1;
    
    sample = tex2D( flowMap, floor((mytexFlowCoord + float2(0.0,0.5)))/ texScale ).rgb;
    flowdir = sample.b * (sample.xy - 0.5);       
    rotmat = float2x2(flowdir.x, -flowdir.y, flowdir.y ,flowdir.x);         
    NormalT0 = tex2D(normalMap, mul(Tcoord, rotmat) - offset*1.33+0.27).rg;
      
    sample = tex2D( flowMap, floor((mytexFlowCoord + float2(0.5,0.5)))/ texScale ).rgb;
    flowdir = sample.b * (sample.xy - 0.5);
    rotmat = float2x2(flowdir.x, -flowdir.y, flowdir.y ,flowdir.x);
    NormalT1 = tex2D(normalMap, mul(Tcoord, rotmat) - offset*1.24).rg ;
      
    float2 NormalTCD = ff.x * NormalT0 + (1.0-ff.x) * NormalT1;    
	float2 NormalT = ff.y * NormalTAB + (1.0-ff.y) * NormalTCD;
    
    NormalT = (NormalT - 0.5) / (ffscale.y * ffscale.x);

    transp = tex2D( flowMap, texFlowCoord ).a;
    NormalT *= 0.04*transp*transp;
 
	myNormal = float3(NormalT,sqrt(1.0-NormalT.x - NormalT.y));

    float3 reflectDir = reflect(EyeDir, myNormal);
    float4 envColor1 = texCUBE(cubeMap, -reflectDir); 
    float4 envColor2 = texCUBE(cubeMap2, -reflectDir); 
    //float4 envColor2 = (envColor1.b, envColor1.g, envColor1.r, 0.7);
	float4 envColor = (envColor1*2 + envColor2)/3;
	
	//je nachdem wie das Licht mal wird, hier noch etwas dran drehen, evtl. auch bei den cubemaps oben
	envColor.r += 0.15;
	envColor.g += 0.15;
	envColor.b += 0.25;
	envColor *= 1.1;
	
	
	myNormal = normalize(Normal + myNormal);
	
	
    myangle = 1-dot(-normalize(EyeDir), myNormal);
    //myangle = 0.95-0.2*myangle*myangle;

    float2 colorCoord;

    colorCoord.x = texColorCoord.x + myNormal.x / texScale2 * 0.03 * transp;
    colorCoord.y = texColorCoord.y + myNormal.y / texScale2 * 0.03 * transp;
	
    float4 base = tex2D(colorMap, colorCoord);

	//float4 base = ((base1+envColor2)/2)*transp;
	

	return lerp(base,envColor,saturate(1.0+pow(myangle,3))*transp);
	
}