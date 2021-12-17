Shader "Custom/Mirror"
{
	Properties
	{
 		_MainTex ("Emissive Texture", 2D) = "black" {}
		_DetailTex ("Detail Texture", 2D) = "white" {}
		_Color ("Detail Tint Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (1,1,1,1)
		_SpecularArea ("Specular Area", Range (0, 0.99)) = 0.1
		_SpecularIntensity ("Specular Intensity", Range (0, 1)) = 0.75
        _ReflectionColor ("Reflection Tint Color", Color) = (1,1,1,1)
		_ReflectionIntensity ("Reflection Intensity", Range (0, 1)) = 0
		
        [Space(20)]
        [Header(Reflection Distortion Properties)]
        [Space(10)]
		_DistortionIntensity ("Distortion Intensity", Range (0, 0.1)) = 0
		_DistortionTex ("Distortion Texture", 2D) = "black" {}
		
	}
	SubShader
	{ 
		Tags { "RenderType"="Opaque" }
		LOD 300
     
		CGPROGRAM

		#pragma surface surf BlinnPhong fullforwardshadows
		#pragma multi_compile __ MIRROR_RECURSION
		#include "UnityCG.cginc"
  
		fixed4 _Color;
		fixed4 _ReflectionColor;
		half _ReflectionIntensity;
		half _SpecularArea;
		half _SpecularIntensity;
		sampler2D _DetailTex;
		sampler2D _NormalTex;
		sampler2D _MainTex;
		
		sampler2D _DistortionTex;
		half _DistortionIntensity;
  
		struct Input
		{
			float2 uv_DetailTex;
			float4 screenPos;
		};
 
		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 detail = tex2D(_DetailTex, IN.uv_DetailTex);
			float4 rval = (tex2D(_DistortionTex, IN.uv_DetailTex) * 2.0f) - 1.0f;

			#if MIRROR_RECURSION

			
			fixed4 refl = tex2D(_MainTex, IN.uv_DetailTex + rval.xy * _DistortionIntensity);

			#else

			IN.screenPos.w = max(0.001, IN.screenPos.w);
			float4 dist = rval * _DistortionIntensity;

			
			float4 sp = UNITY_PROJ_COORD(IN.screenPos - float4(0, dist.y, 0, 0));
			
			fixed4 refl = tex2Dproj(_MainTex, sp);

			#endif
			
			o.Albedo = detail.rgb * _Color.rgb;
			o.Alpha = 1;
			o.Specular = 1.0f - _SpecularArea;
			o.Gloss = _SpecularIntensity;
			o.Emission = (refl.rgb * _ReflectionColor.rgb * _ReflectionIntensity);
		}

		ENDCG
	}
 
	FallBack "Reflective/Specular"
}