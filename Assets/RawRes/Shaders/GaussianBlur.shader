Shader "Hidden/PostProcessLab/GaussianBlur"
{
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        CGINCLUDE
        #include "./Lib/PPLib.hlsl"
        #include "./Lib/Sample.hlsl"
        #include "./Lib/GaussianBlur.hlsl"

        TEXTURE_SAMPLER2D(_MainTex);
        float4 _MainTex_TexelSize;

        vertDownsampleSampleOut downsample_vert(vertIn i)
        {
            return downsample_sample_vert(i.vertex, _MainTex_TexelSize);
        }

        half4 downsample_frag (vertDownsampleSampleOut i) : SV_Target
        {
            return downsample_sample_frag(TEXTURE2D_PARAM(_MainTex), i);
        }
        
        half4 downsample_better_frag (vertOut i) : SV_Target
        {
            return downsample_better_frag(TEXTURE2D_PARAM(_MainTex), i.uv, _MainTex_TexelSize.xy);
        }
        
        gaussianVertOut gaussian_v_vert(vertIn i)
        {
            return gaussian_v_vert(i, _MainTex_TexelSize);
        }

        gaussianVertOut gaussian_h_vert(vertIn i)
        {
            return gaussian_h_vert(i, _MainTex_TexelSize);
        }

        half4 gaussian_frag(gaussianVertOut i) : SV_Target
        {
            return gaussian_frag(TEXTURE2D_PARAM(_MainTex), i.uv, i.offset);
        }

        ENDCG


        Pass
        {
            CGPROGRAM //0 down_sample
            #pragma vertex downsample_vert
            #pragma fragment downsample_frag
            ENDCG
        }
        
        Pass
        {
            CGPROGRAM //1 down_sample better
            #pragma vertex pp_vert 
            #pragma fragment downsample_better_frag 
            ENDCG
        }

        Pass
        {
            CGPROGRAM // 2 v blur
            #pragma vertex gaussian_v_vert
            #pragma fragment gaussian_frag
            ENDCG   
        }
        
        Pass
        {
            CGPROGRAM // 3 h blur
            #pragma vertex gaussian_h_vert
            #pragma fragment gaussian_frag
            ENDCG   
        }
    }
}
