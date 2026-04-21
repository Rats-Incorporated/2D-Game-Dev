Shader "Custom/Shield"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Pulse ("Fade", Range(0,1)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Pulse;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct cords
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            cords vert(appdata v)
            {
                cords o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = v.uv;
                return o;
            }

            float ShieldGlow(float3 VN, float3 VE)
            {
                float bias = 0.04;
                float scale = 2.0;
                float power = 2.8;
                return bias + scale * pow(1.0 - saturate(dot(VE, VN)), power);
            }

            fixed4 frag(cords i) : SV_Target
            {
                float3 VN = normalize(i.worldNormal);
                float3 VE = float3(0, 0, -1);

    
                float fresnel = saturate(ShieldGlow(VN, VE));
              //  float facing = step(0.0, dot(VN, VE));
              //  fresnel *= facing;

                float3 borderColor = float3(0.0, 0.68, 0.92);

                float3 mainColor = float3(0.1, 0.3, 0.92);

                float3 shield = mainColor + borderColor * fresnel ;



                return float4(shield, fresnel*_Pulse);
            }

            ENDCG
        }
    }
}