#ifndef PP_GAUSIANBLUR_LIB
#define PP_GAUSIANBLUR_LIB

#include "PPLib.hlsl"

static const half4 GaussWeight[7] =
{
    half4(0.0205,0.0205,0.0205,0),
    half4(0.0855,0.0855,0.0855,0),
    half4(0.232,0.232,0.232,0),
    half4(0.324,0.324,0.324,1),
    half4(0.232,0.232,0.232,0),
    half4(0.0855,0.0855,0.0855,0),
    half4(0.0205,0.0205,0.0205,0)
};

struct gaussianVertOut
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float2 offset : TEXCOORD1;
};

// = downscale * blur_radius
float _BlurScaleValue;

gaussianVertOut gaussian_v_vert()
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

half4 gaussian_frag(gaussianVertOut i)
{
    return half4(1, 0, 0, 1);
}

#endif //PP_GAUSIANBLUR_LIB