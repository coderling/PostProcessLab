using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PostProcessLab
{
    [EffectSetting(typeof(TestPostEffectRenderer), EffectPoint.BeforeTransparent)]
    [Serializable]
    public sealed class TestPostEffect : EffectBaseSetting
    {
        public override string m_shader
        {
            get
            {
                return "Hidden/TestPostEffect";
            }
        }
    }

    public sealed class TestPostEffectRenderer : EffectRenderer<TestPostEffect>
    {
        public override void Render(RenderContext context, TestPostEffect setting)
        {
            var cmd = context.m_command;
            cmd.BeginSample("TestPostEffect");
            cmd.PostBlitFullScreen(context.m_source, context.m_target, setting.m_mat);
            cmd.EndSample("TestPostEffect");
        }
    }
}
