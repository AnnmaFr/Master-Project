Shader "Custom/DistortedOverlayUI"
{
    Properties
    {
        _PixelDensity ("Pixel Density", Float) = 100.0
        _Threshold ("Flicker Threshold", Float) = 0.5
        _Speed ("Flicker Speed", Float) = 5.0
        _HandRadius ("Hand Effect Radius", Float) = 0.5
    }
    SubShader
    {
        Tags {"Queue"="Overlay" "RenderType"="Transparent" "IgnoreProjector"="True"}
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Ensure transparency
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _PixelDensity;
            float _Threshold;
            float _Speed;
            float _HandRadius;

            float2 _LeftHandPos;
            float2 _RightHandPos;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float random(float2 uv)
            {
                return frac(sin(dot(uv.xy, float2(12.9898,78.233))) * 43758.5453);
            }

            float noise(float2 uv)
            {
                return random(floor(uv * _PixelDensity) + frac(_Time.y * _Speed));
            }

            float handMask(float2 uv, float2 handPos)
            {
                float dist = length(uv - handPos);
                return smoothstep(_HandRadius, _HandRadius * 0.5, dist);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float flicker = noise(i.uv);
                float handEffect = max(handMask(i.uv, _LeftHandPos), handMask(i.uv, _RightHandPos));

                float alpha = (flicker < _Threshold) ? handEffect : 0.0;
                return fixed4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}
