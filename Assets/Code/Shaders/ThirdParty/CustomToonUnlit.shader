Shader "Custom/ToonUnlitURP"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Texture", 2D) = "white" {}
        [MainColor] _Color("Base Color", Color) = (1, 1, 1, 1)
        _TileAmount("Scale Multiplier", Float) = 1
        _ShadowBrightness("Shadow Brightness", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "MainPass"

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _ADDITIONAL_LIGHTS _ADDITIONAL_LIGHTS_VERTEX
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Properties
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_ST;
            float _TileAmount;
            float _ShadowBrightness;
            float4 _Color;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // Transform position to clip space
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                // Get the VertexPositionInputs for the vertex position  
                VertexPositionInputs positions = GetVertexPositionInputs(IN.positionOS.xyz);


                // Convert the vertex position to a position on the shadow map
                float4 shadowCoordinates = GetShadowCoord(positions);


                // Handle UV tiling and offset
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap) * _TileAmount;

                
                
                // OUT.shadowCoords =  shadowCoordinates; // Reserve shadow coordinate

                // // Compute shadow coordinates
                // // TRANSFER_SHADOW(OUT);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample the base texture
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);

                // Calculate shadow attenuation
                // half shadowAttenuation = SHADOW_ATTENUATION(IN);

                // Apply shadow brightness
                baseColor = lerp(baseColor * float4(_ShadowBrightness, _ShadowBrightness, _ShadowBrightness, 1), baseColor, 0.1);

                // Apply the main color
                baseColor *= _Color;

                return baseColor;
            }
            ENDHLSL
        }

        // Shadow Caster Pass (URP Default)
        // Pass
        // {
        //     Name "ShadowCaster"
        //     Tags { "LightMode" = "ShadowCaster" }
        //     HLSLPROGRAM
        //     #pragma vertex UniversalShadowCasterVertex
        //     #pragma fragment UniversalShadowCasterFragment
        //     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        //     ENDHLSL
        // }
        Pass {
            Tags{ "LightMode" = "ShadowCaster" }
            CGPROGRAM
            #pragma vertex VSMain
            #pragma fragment PSMain

            float4 VSMain(float4 vertex : POSITION) : SV_POSITION {
                return UnityObjectToClipPos(vertex);
            }

            float4 PSMain(float4 vertex : SV_POSITION) : SV_TARGET {
                return 0;
            }
         
            ENDCG
        }
    }
}
