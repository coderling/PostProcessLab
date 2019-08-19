#ifndef PP_DEFINE_LIB
#define PP_DEFINE_LIB

#if defined(SHADER_API_D3D9) || defined(SHADER_API_D3D11_9X)
    #define TEXTURE_SAMPLER2D(texture_name) sampler2D texture_name
    #define SAMPLE_TEXTURE2D(texture_name, texcoord) tex2D(texture_name, texcoord)
    #define TEXTURE_ARGS(texture_name) sampler2D texture_name
    #define TEXTURE_PARAM(texture_name) texture_name
#else
    //OpenGL
    #define TEXTURE_SAMPLER2D(texture_name) sampler2D texture_name
    #define SAMPLE_TEXTURE2D(texture_name, texcoord) tex2D(texture_name, texcoord)
    #define TEXTURE_ARGS(texture_name) sampler2D texture_name
    #define TEXTURE_PARAM(texture_name) texture_name
#endif

#endif //PP_DEFINE_LIB