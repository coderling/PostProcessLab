﻿Shader "Hidden/PostProcessLab/Bloom"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        CGINCLUDE
            #include "Lib/PPLib.hlsl"
            #include "Lib/Sample.hlsl"
            #include "./Lib/GaussianBlur.hlsl"
            
            TEXTURE_SAMPLER2D(_MainTex);
            float4 _MainTex_TexelSize;

            TEXTURE_SAMPLER2D(_BloomBlur1);
            float4 _BloomBlur1_TexelSize;

            TEXTURE_SAMPLER2D(_BloomBlur2);
            float4 _BloomBlur2_TexelSize;
            
            vertDownsampleSampleOut downsample_vert(vertIn i)
            {
                return downsample_sample_vert(i.vertex, _MainTex_TexelSize);
            }

            half4 downsample_frag (vertDownsampleSampleOut i) : SV_Target
            {
                return downsample_sample_frag(TEXTURE2D_PARAM(_MainTex), i);
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

            half4 blur_combined(vertOut i) :SV_Target
            {
                half3 col = SAMPLE_TEXTURE2D(_BloomBlur1, i.uv);
                col += SAMPLE_TEXTURE2D(_BloomBlur2, i.uv);
                col *= 0.5f;
                return half4(col, 1);
            }

            half4 bloom_combined(vertOut i) : SV_Target
            {
                half3 col = SAMPLE_TEXTURE2D(_MainTex, i.uv);
                col += SAMPLE_TEXTURE2D(_BloomBlur1, i.uv);
                return half4(col, 1);
            }
        ENDCG
        
        Pass //0 downsample
        {
            CGPROGRAM
            #pragma vertex downsample_vert 
            #pragma fragment 
            ENDCG
        }
        
        Pass //1 v blur
        {
            CGPROGRAM
            #pragma vertex gaussian_v_vert 
            #pragma fragment gaussian_frag 
            ENDCG
        }
        
        Pass //2 h blur
        {
            CGPROGRAM
            #pragma vertex gaussian_h_vert 
            #pragma fragment gaussian_frag 
            ENDCG
        }


        Pass //3 combined 
        {
            CGPROGRAM
            #pragma vertex pp_vert 
            #pragma fragment blur_combined 
            ENDCG
        }
        
        Pass //4 bloom last combined 
        {
            CGPROGRAM
            #pragma vertex pp_vert 
            #pragma fragment bloom_combined 
            ENDCG
        }
    }
}
