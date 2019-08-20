using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PostProcessLab
{
    [Serializable]
    [EffectSetting(typeof(GaussianBlurRenderer), EffectPoint.BeforeFinal)]
    internal class GaussianBlur : EffectBaseSetting 
    {
        public override string m_shader
        {
            get
            {
                return "Hidden/PostProcessLab/GaussianBlur";
            }
        }

        //模糊半径
        public float m_blur_radius = 1.0f;

        //下采样比例指数，2的n次方
        public int m_down_ex = 1;

        //模糊次数
        public int m_blur_times = 1;
    }

    internal sealed class GaussianBlurRenderer : EffectRenderer<GaussianBlur>
    {
        private enum PassIndex
        {
            DS = 0, //downsample
            VBlur,
            HBlur,
        }

        private static readonly int ID_BlurScaleValue = Shader.PropertyToID("_BlurScaleValue");

        public override void Init()
        {

        }

        public override void Render(RenderContext context, GaussianBlur setting)
        {
            var cmd = context.m_command;
            cmd.BeginSample("GaussianBlur");
            //downsample
            float scale = 1.0f / (1 << setting.m_down_ex);
            int blurRT1 = RunTimeHelper.GetTemporaryRTID();
            context.GetScreenScaleTemporaryRT(blurRT1, scale);
            cmd.PostBlitFullScreen(context.m_source, blurRT1, setting.m_mat, (int)PassIndex.DS);
            int blurRT2 = RunTimeHelper.GetTemporaryRTID();
            context.GetScreenScaleTemporaryRT(blurRT2, scale);
            float blurScale = setting.m_blur_radius * scale;
            cmd.SetGlobalFloat(ID_BlurScaleValue, blurScale);

            for(int i = 0; i < setting.m_blur_times; ++i)
            {
                cmd.PostBlitFullScreen(blurRT1, blurRT2, setting.m_mat, (int)PassIndex.VBlur);
                cmd.PostBlitFullScreen(blurRT2, blurRT1, setting.m_mat, (int)PassIndex.HBlur);
            }

            cmd.PostBlitFullScreen(blurRT1, context.m_target);
            cmd.ReleaseTemporaryRT(blurRT1);
            cmd.ReleaseTemporaryRT(blurRT2);

            cmd.EndSample("GaussianBlur");
        }
    }
}
