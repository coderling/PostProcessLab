#ifndef PP_SAMPLE_LIB
#define PP_SAMPLE_LIB

#include "PPLib.hlsl"

//downsampling sample
struct vertDownsampleSampleIn
{
    float4 vertex : POSITION;    
};

struct vertDownsampleSampleOut
{
    float4 vertex : SV_POSITION;
    float2 uv0 : TEXCOORD0;
    float2 uv1 : TEXCOORD1;
    float2 uv2 : TEXCOORD2;
    float2 uv3 : TEXCOORD3;
};

vertDownsampleSampleOut downsample_sample_vert(float4 vertex, float4 texel_size)
{
    vertDownsampleSampleOut o;
    o.vertex = TransVertexClip(vertex);
    float2 uv = TransVertexUV(vertex);
    float4 offset = texel_size.xyxy * float4(0.5, 0.5, -0.5, -0.5);
    o.uv0 = uv + offset.xy;
    o.uv1 = uv + offset.zw;
    o.uv2 = uv + offset.xz;
    o.uv3 = uv + offset.zy;

    return o;
}

half4 downsample_sample_frag(TEXTURE2D_ARGS(tex), vertDownsampleSampleOut i)
{
    half4 col = SAMPLE_TEXTURE2D(tex, i.uv0);
    col += SAMPLE_TEXTURE2D(tex, i.uv1);
    col += SAMPLE_TEXTURE2D(tex, i.uv2);
    col += SAMPLE_TEXTURE2D(tex, i.uv3);

    return col * 0.25;
}

half4 downsample_box_frag(TEXTURE2D_ARGS(tex), float2 uv, float2 texel_size)
{
    float4 offset = texel_size.xyxy * float4(1.0, 1.0, -1.0, -1.0);

    half4 col = SAMPLE_TEXTURE2D(tex, uv + offset.xy);
    col += SAMPLE_TEXTURE2D(tex, uv + offset.zw);
    col += SAMPLE_TEXTURE2D(tex, uv + offset.xz);
    col += SAMPLE_TEXTURE2D(tex, uv + offset.zy);

    return col * 0.25;
}

// Better, temporally stable box filtering
// [Jimenez14] http://goo.gl/eomGso
// . . . . . . .
// . A . B . C .
// . . D . E . .
// . F . G . H .
// . . I . J . .
// . K . L . M .
// . . . . . . .
//emmmm i can't feel better
half4 downsample_better_frag(TEXTURE2D_ARGS(tex), float2 uv, float2 texelSize)
{
    half4 A = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2(-1.0, -1.0));
    half4 B = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2( 0.0, -1.0));
    half4 C = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2( 1.0, -1.0));
    half4 D = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2(-0.5, -0.5));
    half4 E = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2( 0.5, -0.5));
    half4 F = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2(-1.0,  0.0));
    half4 G = SAMPLE_TEXTURE2D(tex, uv                                 );
    half4 H = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2( 1.0,  0.0));
    half4 I = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2(-0.5,  0.5));
    half4 J = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2( 0.5,  0.5));
    half4 K = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2(-1.0,  1.0));
    half4 L = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2( 0.0,  1.0));
    half4 M = SAMPLE_TEXTURE2D(tex, uv + texelSize * float2( 1.0,  1.0));

    half2 div = (1.0 / 4.0) * half2(0.5, 0.125);

    half4 o = (D + E + I + J) * div.x;
    o += (A + B + G + F) * div.y;
    o += (B + C + H + G) * div.y;
    o += (F + G + L + K) * div.y;
    o += (G + H + M + L) * div.y;

    return o;
}

#endif //PP_SAMPLE_LIB