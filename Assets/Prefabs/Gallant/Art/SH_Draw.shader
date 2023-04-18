Shader "Unlit/SH_CorruptDraw"
{
    Properties
    {
        _MainTex ("MainTexture", 2D) = "white" {}
        _MaskTex ("MaskTexture", 2D) = "white" {}
        _DrawCoordinate ("Draw", Vector) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPos : POSITION;
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(unity_ObjectToWorld,v.vertex);
                o.uv = v.uv;
                float4 uv = float4(0,0,0,1);
                uv.xy = (v.uv.xy * 2 - 1) * float2(1, _ProjectionParams.x);
                o.vertex = uv;
                return o;
            }

            float mask(float3 position, float3 center, float radius, float hardness)
            {
                float m = distance(center,position);
                return 1 - smoothstep(radius*hardness, radius, m);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float m = mask(i.worldPos, _PainterPosition, _Radius, _Hardness);
                float edge = m *_Strength;
                return lerp(float4(0,0,0,0), float4(1,0,0,1), edge);
            }
            ENDCG
        }
    }
}
