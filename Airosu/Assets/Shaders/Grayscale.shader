Shader "Unlit/Grayscale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GrayScale ("Apply Gray Shader", Range(0, 1)) = 0
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GrayScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                if (_GrayScale == 1) {
                    float redPercentage = 0.299;
                    float greenPercentage = 0.587;
                    float bluePercentage = 0.144;
                    
                    float newRed = col.x * redPercentage;
                    float newGreen = col.y * greenPercentage;
                    float newBlue = col.z * bluePercentage;

                    float intensity = newRed + newGreen + newBlue;
                    fixed4 newColor = fixed4(intensity, intensity, intensity, col.w);
                    return newColor;
                }
                return col;
            }
            ENDCG
        }
    }
}
