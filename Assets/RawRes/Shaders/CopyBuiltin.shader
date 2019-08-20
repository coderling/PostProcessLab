Shader "Hidden/PostProcessLab/CopyBuiltIn"
{
    //cmd.Blit 必须定义Properties才可以
    Properties
    {
        _MainTex ("", 2D) = "white" {}
    }
    
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        CGINCLUDE
        #include "./Lib/PPLib.hlsl"

        
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
        
        TEXTURE_SAMPLER2D(_MainTex);
        float4 _MainTex_TexelSize;
        
        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 col = SAMPLE_TEXTURE2D(_MainTex, i.uv);
            return col;
        }
        ENDCG

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
}
