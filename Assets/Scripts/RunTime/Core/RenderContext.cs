using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PostProcessLab
{
    public class RenderContext
    {
        private Camera _m_camera;
        internal Camera m_camera
        {
            get
            {
                return _m_camera;
            }

            set
            {
                _m_camera = value;
            }
        }


        internal CommandBuffer m_command;
        internal RenderTargetIdentifier m_source;
        internal RenderTargetIdentifier m_target;
        internal int m_width = 0;
        internal int m_height = 0;

        public void Reset()
        {
            m_camera = null;
            m_command = null;
            m_source = new RenderTargetIdentifier(0, 0);
            m_target = new RenderTargetIdentifier(0, 0);
            m_width = m_height = 0;
        }

        private void OnSetCamera()
        {
            m_width = m_camera.pixelWidth;
            m_height = m_camera.pixelHeight;
        }

        public void GetFullscreenTemporaryRT(CommandBuffer cmd, int nameID, RenderTextureFormat format, FilterMode filter, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, int depthBufferBits = 0, int width = 0, int height = 0)
        {
            int w = width > 0 ? width : m_width;
            int h = height > 0 ? height : m_height;
            cmd.GetTemporaryRT(nameID, w, h, depthBufferBits, filter, format, readWrite);
        }

        public void GetFullscreenTemporaryRT(int nameID, RenderTextureFormat format, FilterMode filter, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, int depthBufferBits = 0, int width = 0, int height = 0)
        {
            GetFullscreenTemporaryRT(m_command, nameID, format, filter, readWrite, depthBufferBits, width, height);
        }
    }
}
