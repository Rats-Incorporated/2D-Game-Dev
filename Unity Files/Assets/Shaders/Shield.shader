Shader "Custom/Shield"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Ka ("Ambient", Float) = 0.2
        _Kd ("Diffuse", Float) = 1.0
        _Ks ("Specular", Float) = 1.0
        _Shininess ("Shininess", Float) = 32
        _TimeScale ("Time", Float) = 1.0
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
            float _Ka, _Kd, _Ks, _Shininess, _TimeScale;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = v.uv;
                return o;
            }

            float3 Fresnel(float3 N, float3 V)
            {
                float R0 = 0.14;
                float cosTheta = saturate(dot(V, N));
                float R = R0 + (1.0 - R0) * pow(1.0 - cosTheta, 5.0);
                return R;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 N = normalize(i.worldNormal);
                float3 V = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 L = normalize(_WorldSpaceLightPos0.xyz);

    
                float fresnel = Fresnel(N, V);

                float3 fresnelColor = float3(0.0, 0.7, 0.9);
                float3 baseColor = float3(0.5, 0.2, 0.9);

                float3 shield = fresnelColor * fresnel;


                float2 uv = i.uv * 25.0;
                float2 grid = frac(uv) - 0.5;
  

                float NdotL = saturate(dot(N, L));
                float3 diffuse = _Kd * NdotL * baseColor;
                float3 ambient = _Ka * baseColor;

                float3 R = reflect(-L, N);
                float spec = pow(saturate(dot(R, V)), _Shininess);
                float3 specular = _Ks * spec * float3(1,1,1);

                float3 finalColor =
                    ambient +
                    diffuse +
                    specular +
                    shield;

                return float4(finalColor, fresnel);
            }

            ENDCG
        }
    }
}