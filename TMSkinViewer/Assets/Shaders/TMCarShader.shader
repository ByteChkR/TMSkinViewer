Shader "Custom/TMCarShader"
{
    Properties
    {
        [Header(Main Properties)]
        [Space(10)]
        _Color ("Color", Color) = (1,1,1,1) //Done
        _MainTex ("Albedo (RGB)", 2D) = "white" {} //Done
        _Normals ("Normal", 2D) = "bump" {} //Done
        _AO ("Ambient Occlusion", 2D) = "white" {} //Done
        _Emission ("Emission (RGB)", 2D) = "black" {}
        
        [Space(20)]
        [Header(Dirt Mask Properties)]
        [Space(10)]
        _DirtIntensity ("Dirt Intensity", Range(0,1)) = 0.5
        _DirtMask ("Dirt Mask", 2D) = "white" {}
        _DirtColor("Dirt Color", Color) = (1,0.9176471,0.6,0.4823529)
        _Roughness ("Roughness", 2D) = "white" {} //Done
        _TurboColor ("Turbo Color", Color) = (1,1,1,1)
        
        
        [Space(20)]
        [Header(Clear Coat Properties)]
        [Space(10)]
        _ClearCoat ("Clear Coat", 2D) = "white" {} //Done
        _ClearCoatIntensity ( "Clear Coat Intensity", Range(0,1)) = 1.0 //Done
        
        
        [Space(20)]
        [Header(Car Control Properties)]
        [Space(10)]
        _BrakeLights ("Brake Lights", Range(0,2)) = 0
        _Energy ("Energy", Range(0,1)) = 0
        _BrakeHeat ("Brake Heat", Range(0,1)) = 0
        _SelfIllumination ("Self Illumination", Range(0,1)) = 0
        _FrontLights ("Front Lights", Range(0,2)) = 0
        _TurboIntensity ("Turbo Intensity", Range(0,1)) = 0
        _ExhaustHeat ("Exhaust Heat", Range(0,1)) = 0
        _Boost ("Boost", Range(0,1)) = 0
        _SelfIlluminationNight ("Self Illumination Night", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        
        struct Input
        {
            float2 uv_MainTex;
        };


        //Albedo
        sampler2D _MainTex;
        fixed4 _Color;
        
        sampler2D _Roughness;
        sampler2D _Normals;
        sampler2D _AO;

        //Dirt Mask
        sampler2D _DirtMask;
        fixed _DirtIntensity;
        fixed4 _DirtColor;
        fixed4 _TurboColor;


        //Emission
        sampler2D _Emission;
        fixed _BrakeLights;
        fixed _Energy;
        fixed _BrakeHeat;
        fixed _SelfIllumination;
        fixed _FrontLights;
        fixed _TurboIntensity;
        fixed _ExhaustHeat;
        fixed _Boost;
        fixed _SelfIlluminationNight;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 d = tex2D (_DirtMask, IN.uv_MainTex);
            fixed4 r = tex2D (_Roughness, IN.uv_MainTex);
            
            //Combine Main Tex and Dirt Mask
            fixed4 albedo = lerp (c, _DirtColor, d.r * _DirtIntensity);
            fixed smoothness = lerp(1-r.r, 0, d.r * _DirtIntensity);
            fixed metallic = lerp(r.g, 0, d.r * _DirtIntensity);
            
            o.Albedo = albedo.rgb;
            o.Metallic = metallic;
            o.Smoothness = smoothness;
            o.Normal = UnpackNormal(tex2D (_Normals, IN.uv_MainTex)).rgb;
            o.Occlusion = tex2D (_AO, IN.uv_MainTex).r;
            o.Alpha = c.a;
                        
            //Emission
            fixed4 em = tex2D (_Emission, IN.uv_MainTex);
            fixed emAlpha = em.a;
            if(emAlpha == 0)
            {
                o.Emission = em.rgb * _BrakeLights;
            }
            else if(emAlpha >= 0.0625 && emAlpha < 0.185)
            {
                o.Emission = em.rgb * _Energy;
            }
            else if(emAlpha >= 0.185 && emAlpha < 0.315)
            {
                o.Emission = em.rgb * _BrakeHeat;
            }
            else if(emAlpha >= 0.315 && emAlpha < 0.445)
            {
                o.Emission = em.rgb * _SelfIllumination;
            }
            else if(emAlpha >= 0.445 && emAlpha < 0.565)
            {
                o.Emission = em.rgb * _FrontLights;
            }
            else if(emAlpha >= 0.565 && emAlpha < 0.695)
            {
                o.Emission = em.rgb * _TurboColor * _TurboIntensity;
            }
            else if(emAlpha >= 0.695 && emAlpha < 0.815)
            {
                o.Emission = em.rgb * _ExhaustHeat;
            }
            else if(emAlpha >= 0.815 && emAlpha < 0.945)
            {
                o.Emission = em.rgb * _Boost;
            }
            else
            {
                o.Emission = em.rgb * _SelfIlluminationNight;
            }
        }
        ENDCG
        
        CGPROGRAM

        struct Input
        {
            float2 uv_MainTex;
        };
        
        sampler2D _ClearCoat;
        fixed _ClearCoatIntensity;

        #pragma surface surf StandardSpecular nofog alpha:premul exclude_path:deferred exclude_path:prepass
        #pragma target 3.0

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            fixed r = 1 - (1 - _ClearCoatIntensity) * tex2D(_ClearCoat, IN.uv_MainTex).r;
            o.Smoothness = r;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}
