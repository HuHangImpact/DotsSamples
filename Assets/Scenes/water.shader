// Define the shader as a surface shader
Shader "Custom/Water"  // Add the missing shader name
{
    Properties
    {
        _Color ("Water Color", Color) = (1,1,1,1)
        _MainTex ("Water Texture", 2D) = "white" {}
        _Distortion ("Distortion", Range(0,1)) = 0.1
        _Speed ("Speed", Range(0,1)) = 0.1
        // Add the missing _Reflectivity property
        _Reflectivity ("Reflectivity", Range(0,1)) = 1
        // Add the missing _Refraction property
        _Refraction ("Refraction", Range(0,1)) = 1
        // Add the missing _Cube property
        _Cube ("Reflection Cubemap", Cube) = "_Skybox" {}
    }

    SubShader
    {
        Tags
        {   
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalPipeline"  // Add the missing tag
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Declare properties
            float4 _Color;
            float _Distortion;
            float _Speed;
            // Add the missing _Reflectivity property
            float _Reflectivity;
            // Add the missing _Refraction property
            float _Refraction;
            // Add the missing _Cube property
            samplerCUBE _Cube;
            sampler2D _MainTex;

    
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Define the vertex shader function
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Define the fragment shader function
            fixed4 frag (v2f i) : SV_Target
            {
                // Apply the water texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // Apply the distortion effect
                float2 distortion = _Distortion * (tex2D(_MainTex, i.uv * 10 + _Time.y * _Speed).rg * 2 - 1);
                i.vertex.xy += distortion;

                // Calculate the reflection and refraction vectors
                float3 worldNormal = normalize(UnityObjectToWorldNormal(i.vertex.xyz));
                float3 worldView = normalize(UnityWorldSpaceViewDir(i.vertex.xyz));
                float3 worldRefl = reflect(worldView, worldNormal);
                float3 worldRefr = refract(worldView, worldNormal, 1.0 / 1.33);

                // Apply the reflection and refraction
                col.rgb += texCUBE(_Cube, worldRefl).rgb * _Reflectivity;
                col.rgb += texCUBE(_Cube, worldRefr).rgb * _Refraction;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}


