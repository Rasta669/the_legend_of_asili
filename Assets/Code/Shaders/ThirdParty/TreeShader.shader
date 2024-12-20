Shader "Custom/TreeShader"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Texture", 2D) = "white" {}
        [MainColor] _Color("Base Color", Color) = (1,1,1,1)
        _FresnelPower ("Fresnel Power", Float) = 1.0
        _FresnelColor ("Fresnel Color", Color) = (0,1,0,1)
        _Contrast ("Contrast", Float) = 4.0
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        Pass    
        {
            Name "MainPass"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            // Define texture and sampler for SRP batcher compatibility
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            // Declare material properties inside CBUFFER
            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float4 _BaseMap_ST; // Tiling and offset
            float _FresnelPower;
            float4 _FresnelColor;
            float _Contrast;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // Transform position to clip space
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                // Transform normal and view direction to world space
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normalOS);

                // Ensure IN.positionOS is promoted to a float4 before calling TransformObjectToWorld
                float4 worldPos4 = float4(IN.positionOS.xyz, 1.0);
                float3 worldPos = TransformObjectToWorld(worldPos4).xyz;
                OUT.viewDirWS = GetCameraPositionWS() - worldPos;

                // Handle tiling and offset for UVs
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

                return OUT;
            }


            half4 frag(Varyings IN) : SV_Target
            {
                // Sample the texture with tiling and offset
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _Color;

                // Calculate the Fresnel effect
                float fresnelFactor = 1.0 - abs(dot(normalize(IN.worldNormal), normalize(IN.viewDirWS)));
                float fresnelPowerAdjusted = pow(abs(fresnelFactor), _FresnelPower);
                float contrastAdjusted = pow(fresnelPowerAdjusted, _Contrast);

                // Interpolate between base color and Fresnel color
                half4 finalColor = lerp(baseColor, _FresnelColor, contrastAdjusted);

                // Apply alpha and handle transparency
                finalColor.a = baseColor.a;
                clip(finalColor.a - 0.5); // Alpha clipping
                return finalColor;
            }
            ENDHLSL
        }
    }
}
