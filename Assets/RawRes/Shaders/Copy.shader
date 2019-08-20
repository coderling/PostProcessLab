Shader "Hidden/PostProcessLab/Copy"
{
    
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        CGINCLUDE
        #include "./Lib/PPLib.hlsl"

        TEXTURE_SAMPLER2D(_MainTex);
        float4 _MainTex_TexelSize;

        fixed4 frag(vertOut i) : SV_Target
        {
            fixed4 col = SAMPLE_TEXTURE2D(_MainTex, i.uv);
            return col;
        }
        ENDCG
        
        Pass
        {
            CGPROGRAM
            #pragma vertex pp_vert
            #pragma fragment frag
            ENDCG
        }
    }
}
