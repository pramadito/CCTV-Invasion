Shader "Custom/FPV" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
	_BumpMap("Bumpmap", 2D) = "bump" {}
	_BumpIntensity("Bump Intensity", Range(0.0, 1.0)) = 0.0
		_SpecularMap("Specular Map", 2D) = "white" {}
	_Shininess("Shininess", Range(0.01, 8)) = 0.3
		_SpecularIntensity("Specular Intensity", Range(0.0, 16.0)) = 4.0
		_Cube("Reflective Cubemap", CUBE) = ""
		_ReflectIntensity("Reflectiveness", Float) = 0.0
		_CubeBlur("Glossiness", Float) = 0.0
		_EmissiveMap("Emissive", 2D) = "white" {}
	_EmissiveTint("Emissive Tint", Color) = (1,1,1,1)
		_EmissiveIntensity("Emissive Intensity", Range(0.0, 16.0)) = 0.0
		_ReflSat("Reflection Desaturation", Range(0,1)) = 0.0
	}

		CGINCLUDE
#pragma multi_compile __ FIRSTPERSONVIEW
#pragma multi_compile __ FPV_Light
		ENDCG

		SubShader{
		Tags{ "RenderType" = "FPV" }
		LOD 200

		CGPROGRAM
#pragma target 3.0
#pragma surface surf ToonRamp

		sampler2D _Ramp;
	sampler2D _BumpMap;
	sampler2D _SpecularMap;
	sampler2D _EmissiveMap;

	half _SpecularIntensity;
	half _BumpIntensity;
	half _EmissiveIntensity;

	half3 specularColor;

	half _ReflectIntensity;
	half3 reflectionColor;
	half _ReflSat;
	float _CubeBlur;

	half _Shininess;

	sampler2D _MainTex;
	samplerCUBE _Cube;
	float4 _Color;
	float4 _EmissiveTint;

#pragma lighting ToonRamp

	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif

		half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
		half3 ramp = tex2D(_Ramp, float2(d,0)).rgb;

		fixed3 reflection = lerp(reflectionColor.rgb, dot(reflectionColor.rgb, float3(0.3, 0.59, 0.11)), _ReflSat) * _ReflectIntensity;// * 0.5;

		fixed nh = max(0, dot(s.Normal, normalize(lightDir + viewDir)));
		fixed spec = pow(nh, _Shininess * 128) * _Shininess;

		fixed3 specFinal = (spec * specularColor) + (reflection * specularColor * s.Albedo);

		half4 c;
		c.a = s.Alpha;
		c.rgb = ((s.Albedo * _LightColor0.rgb * ramp) + ((_LightColor0.rgb + unity_AmbientSky) * _SpecularIntensity * specFinal)) * (atten * 2);

		return c;
	}

	struct Input {
		float2 uv_MainTex : TEXCOORD0;
		float2 uv_BumpMap;
		float2 uv_SpecularMap;
		float2 uv_EmissiveMap;
		float3 viewDir;
		float3 worldRefl; INTERNAL_DATA
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

		o.Albedo = c.rgb;
		o.Alpha = c.a;
		o.Normal = lerp(UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)), fixed3(0,0,1), -_BumpIntensity + 1);
		specularColor = tex2D(_SpecularMap, IN.uv_SpecularMap).rgb;
		reflectionColor = texCUBElod(_Cube, float4(WorldReflectionVector(IN, o.Normal), _CubeBlur)).rgb;
		o.Emission = ((tex2D(_EmissiveMap, IN.uv_EmissiveMap) * _EmissiveTint) * _EmissiveIntensity);
	}
	ENDCG

	}

		Fallback "Diffuse"
}