Shader "Hidden/PostProcessLab/GaussianBlur"
{
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        CGINCLUDE
        #include "./Lib/PPLib.hlsl"
        #include "./Lib/Sample.hlsl"

        TEXTURE_SAMPLER2D(_MainTex);
        float4 _MainTex_TexelSize;

        vertDownsampleSampleOut downsample_vert(vertIn i)
        {
            return downsample_sample_vert(i.vertex, _MainTex_TexelSize);
        }

        half4 downsample_frag (vertDownsampleSampleOut i) : SV_Target
        {
            return downsample_sample_frag(TEXTURE_PARAM(_MainTex), i);
        }
        ENDCG

        Pass
        {
            CGPROGRAM //0 down_sample
            #pragma vertex downsample_vert
            #pragma fragment downsample_frag
            ENDCG
        }
    }
}
