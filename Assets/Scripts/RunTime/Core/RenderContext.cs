using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PostProcessLab
{
    public class RenderContext
    {
        internal Camera m_camera;
        internal CommandBuffer m_command;
        internal RenderTargetIdentifier m_source;
        internal RenderTargetIdentifier m_target;

        public void Reset()
        {
            m_camera = null;
            m_command = null;
            m_source = new RenderTargetIdentifier(0, 0);
            m_target = new RenderTargetIdentifier(0, 0);
        }
    }
}
