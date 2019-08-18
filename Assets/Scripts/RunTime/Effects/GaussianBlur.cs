using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PostProcessLab
{
    [Serializable]
    [EffectSetting(typeof(GaussianBlurRenderer), EffectPoint.BeforeFinal)]
    public class GaussianBlur : EffectBaseSetting 
    {
        public override string m_shader
        {
            get
            {
                return "Hidden/PostProcessLab/GaussianBlur";
            }
        }

        
    }

    public class GaussianBlurRenderer : EffectRendererBase
    {
        public override void BaseRender(RenderContext context, EffectBaseSetting setting)
        {
            var cmd = context.m_command;
            cmd.BeginSample("GaussianBlur");
            //cmd.PostBlit(context.m_source, context.m_target);
            cmd.PostBlitFullScreen(context.m_source, context.m_target, setting.m_mat);
            cmd.EndSample("GaussianBlur");
        }
    }
}
