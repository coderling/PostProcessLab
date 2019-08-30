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
    }

    internal sealed class BloomRender : EffectRenderer<Bloom>
    {
        public override void Render(RenderContext context, Bloom setting)
        {
            var cmd = context.m_command;
            cmd.BeginSample("Bloom");

            cmd.EndSample("Bloom");
        }
    }
}
