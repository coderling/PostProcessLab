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
                if(value != null)
                {
                    OnSetCamera();
                }
            }
        }


        internal CommandBuffer m_command;
        internal RenderTargetIdentifier m_source;
        internal RenderTargetIdentifier m_target;
        internal int m_width = 0;
        internal int m_height = 0;
        internal float m_radio = 1f;

        internal void Reset()
        {
            m_camera = null;
            m_command = null;
            m_source = new RenderTargetIdentifier(0, 0);
            m_target = new RenderTargetIdentifier(0, 0);
            m_width = m_height = 0;
            m_radio = 1f;
        }

        private void OnSetCamera()
        {
            m_width = _m_camera.pixelWidth;
            m_height = _m_camera.pixelHeight;
            m_radio = 1.0f * m_height / m_width;
        }

        internal void GetScreenScale(float scale, out int w, out int h)
        {
            w = Mathf.CeilToInt(m_width * scale);
            h = Mathf.CeilToInt(w * m_radio); 
        }

        internal void GetFullscreenTemporaryRT(CommandBuffer cmd, int nameID, RenderTextureFormat format, FilterMode filter, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, int depthBufferBits = 0, int width = 0, int height = 0)
        {
            int w = width > 0 ? width : m_width;
            int h = height > 0 ? height : m_height;
            cmd.GetTemporaryRT(nameID, w, h, depthBufferBits, filter, format, readWrite);
        }

        public void GetFullscreenTemporaryRT(int nameID, RenderTextureFormat format, FilterMode filter, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, int depthBufferBits = 0, int width = 0, int height = 0)
        {
            GetFullscreenTemporaryRT(m_command, nameID, format, filter, readWrite, depthBufferBits, width, height);
        }

        public void GetScreenScaleTemporaryRT(int nameID, float scale, RenderTextureFormat format = RenderTextureFormat.Default,
                    FilterMode filter = FilterMode.Bilinear, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, int depthBufferBits = 0)
        {
            int w, h;
            GetScreenScale(scale, out w, out h);
            GetFullscreenTemporaryRT(m_command, nameID, format, filter, readWrite, depthBufferBits, w, h);
        }
    }
}
