#ifndef PP_GAUSIANBLUR_LIB
#define PP_GAUSIANBLUR_LIB

#include "PPLib.hlsl"

#define FILTER_SIZE 5

//9 Tap Filter
static const half GaussWeight[5] =
{
    0.2270270270, // 0
    0.1945945946, // 1 / -1
    0.1216216216, // 2 / -2
    0.0540540541, // 3 / -3
    0.0162162162  // 4 / -4
};

struct gaussianVertOut
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float2 offset : TEXCOORD1;
};

// = downscale * blur_radius
float _BlurScaleValue;

gaussianVertOut gaussian_v_vert(vertIn i, float2 texel_size)
{
    gaussianVertOut o;
    o.vertex = TransVertexClip(i.vertex);
    o.uv = TransVertexUV(i.vertex);
    o.offset = texel_size.xy * float2(0, 1.0) * _BlurScaleValue;
    return o;   
}

gaussianVertOut gaussian_h_vert(vertIn i, float2 texel_size)
{
    gaussianVertOut o;
    o.vertex = TransVertexClip(i.vertex);
    o.uv = TransVertexUV(i.vertex);
    o.offset = texel_size.xy * float2(1.0, 0) * _BlurScaleValue;
    return o;   
}

half4 gaussian_frag(TEXTURE2D_ARGS(tex), float2 uv, float2 offset)
{
    half4 col = SAMPLE_TEXTURE2D(tex, uv) * GaussWeight[0];
    float2 t_offset = offset;
    for(int i = 1; i < FILTER_SIZE; i++)
    {
        col += SAMPLE_TEXTURE2D(tex, uv + t_offset) * GaussWeight[i];
        col += SAMPLE_TEXTURE2D(tex, uv - t_offset) * GaussWeight[i];
        t_offset += offset; 
    }

    return col;
}

#endif //PP_GAUSIANBLUR_LIB