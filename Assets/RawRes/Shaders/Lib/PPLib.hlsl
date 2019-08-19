#ifndef PPLIB_LIB
#define PPLIB_LIB

#include "UnityCG.cginc"
#include "Define.hlsl"

struct vertIn
{
    float4 vertex : POSITION;
};

struct vertOut
{
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
};

float4 TransVertexClip(float4 vertex)
{
    return float4(vertex.xy, 0, 1);
}

float2 TransVertexUV(float4 vertex)
{
    float2 uv = 0.5 * (vertex.xy + 1.0);

#if UNITY_UV_STARTS_AT_TOP
    uv = uv * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif

    return uv;
}

vertOut pp_vert(vertIn i)
{
    vertOut o;
    o.vertex = TransVertexClip(i.vertex);
    o.uv = TransVertexUV(i.vertex);
    return o;
}

#endif //PPLIB_LIB