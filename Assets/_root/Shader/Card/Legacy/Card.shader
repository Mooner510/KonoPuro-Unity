Shader "Custom/Card"
{
    Properties
    {
        _Color ("Tint", Color) = (0, 0, 0, 1)
        [PreRenderData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _BackTex ("Back Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            float4 _Color;
            sampler2D _MainTex;
            sampler2D _BackTex;
            float4 _MainTex_ST;
            float4 _BackTex_ST;

            v2f vert(appdata_base v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;

                return o;
            }

            fixed4 frag(v2f i, fixed facing : VFACE) : SV_TARGET
            {
                if (facing > 0)
                {
                    return tex2D(_MainTex, i.texcoord) * _Color;
                }
                i.texcoord[0] = 1 - i.texcoord[0];
                // i.texcoord[1] = 1 - i.texcoord[1];
                return tex2D(_BackTex, i.texcoord) * _Color;
            }
            ENDCG
        }
    }
}