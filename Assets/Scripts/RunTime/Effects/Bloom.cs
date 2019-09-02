using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// refs:
/// [1]https://software.intel.com/en-us/articles/compute-shader-hdr-and-bloom/
/// [2]https://learnopengl.com/Advanced-Lighting/Bloom
/// </summary>

namespace PostProcessLab
{
    [Serializable]
    [EffectSetting(typeof(BloomRender), EffectPoint.BeforeFinal)]
    public class Bloom : EffectBaseSetting
    {
        public override string m_shader => "Hidden/PostProcessLab/Bloom";


        //模糊半径
        [Range(1, 10)]
        public float m_blur_radius = 1.0f;

        [Range(0, 2)]
        public float m_threthold = 1.0f;

        [Range(0, 3)]
        public float m_Strength = 1.0f;
    }

    internal sealed class BloomRender : EffectRenderer<Bloom>
    {
        private enum EBloomPass
        {
            DSample = 0,
            VBlur = 1,
            HBlur = 2,
            Combined = 3,
            BloomCombined = 4
        }

        private readonly int ID_BlurScaleValue = Shader.PropertyToID("_BlurScaleValue");
        private readonly int ID_Threthold= Shader.PropertyToID("_Threthold");
        private readonly int ID_BloomBlur = Shader.PropertyToID("_BloomBlur");
        private readonly int ID_Strength = Shader.PropertyToID("_Strength");

        public override void Render(RenderContext context, Bloom setting)
        {
            var cmd = context.m_command;
            cmd.BeginSample("Bloom");
            cmd.SetGlobalFloat(ID_Threthold, setting.m_threthold);
            int w;
            int h;


            int lastRes = -1;
            int temp1 = RunTimeHelper.GetTemporaryRTID();
            int temp2 = RunTimeHelper.GetTemporaryRTID();
            for (int i = 4; i > 1; i--)
            {
                int scale = 1 << i;
                w = context.m_width >> i;
                h = context.m_height >> i;
                context.GetFullscreenTemporaryRT(temp1, RenderTextureFormat.Default, FilterMode.Bilinear, RenderTextureReadWrite.Default, 0, w, h);
                context.GetFullscreenTemporaryRT(temp2, RenderTextureFormat.Default, FilterMode.Bilinear, RenderTextureReadWrite.Default, 0, w, h);
                cmd.PostBlitFullScreen(context.m_source, temp1, setting.m_mat, (int)EBloomPass.DSample);

                cmd.SetGlobalFloat(ID_BlurScaleValue, setting.m_blur_radius / scale);
                cmd.PostBlitFullScreen(temp1, temp2, setting.m_mat, (int)EBloomPass.VBlur);
                cmd.PostBlitFullScreen(temp2, temp1, setting.m_mat, (int)EBloomPass.HBlur);
                
                if(lastRes > 0)
                {
                    cmd.SetGlobalTexture(ID_BloomBlur, temp1);
                    cmd.PostBlitFullScreen(lastRes, temp2, setting.m_mat, (int)EBloomPass.Combined);
                    cmd.ReleaseTemporaryRT(temp1);
                    cmd.ReleaseTemporaryRT(lastRes);
                    lastRes = temp2;
                }
                else
                {
                    cmd.ReleaseTemporaryRT(temp2);
                    lastRes = temp1;
                }
            }


            cmd.SetGlobalFloat(ID_Strength, setting.m_Strength);
            cmd.SetGlobalTexture(ID_BloomBlur, lastRes);
            cmd.PostBlitFullScreen(context.m_source, context.m_target, setting.m_mat, (int)EBloomPass.BloomCombined);
            cmd.ReleaseTemporaryRT(lastRes);
            cmd.EndSample("Bloom");
        }
    }
}
