Shader "Custom/S_MotionLines"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [SpeedScale] _speedScale("Speed Scale", float) = 16.0
        [ClipPosition] _clipPosition("Clip Position", float) = 0.2
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue"= "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        ZTest Always

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float1 _speedScale;
            float1 _clipPosition;

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
            CBUFFER_END

            float random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float alpha = 0.0;
                float2 pos = IN.uv - float2(0.5,0.5);
                float theta = round(64.0 * atan2(pos.y, pos.x));
                float dist = length(pos);
                float distValue = round(dist * 4.0 + _Time.y * -_speedScale + 8.0 * random(float2(theta, 0.0)));
                if ( dist > _clipPosition + random(float2(theta, 0.0)) * 0.3 &&
                random(float2(theta, distValue)) < 0.02)
                {
                    alpha = 0.2;
                }
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                color.a *= alpha;
                return color;
            }
            ENDHLSL
        }
    }
}
