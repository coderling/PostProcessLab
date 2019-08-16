using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

namespace PostProcessLab
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class EffectLayer : MonoBehaviour
    {
        public EffectLayerProfile m_profile;
        internal Dictionary<EffectPoint, List<EffectBaseSettingWrap>> m_activeEffectsDic { get; private set; }
        public Dictionary<Type, EffectRendererBase> m_activeRendererDic { get; private set; }

        private RenderContext m_context;
        private Camera m_camera;
        private CommandBuffer m_opaqueCommandBuffer;
        private CommandBuffer m_imageEffectCommandBuffer;

        private void Awake()
        {
            m_context = new RenderContext();
            m_camera = this.GetComponent<Camera>();
            m_opaqueCommandBuffer = new CommandBuffer();
            m_imageEffectCommandBuffer = new CommandBuffer();

            m_camera.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, m_opaqueCommandBuffer);
            m_camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, m_imageEffectCommandBuffer);

            m_activeEffectsDic = new Dictionary<EffectPoint, List<EffectBaseSettingWrap>>();
            m_activeRendererDic = new Dictionary<Type, EffectRendererBase>();
            if(m_profile != null)
            {
                foreach(var setting in m_profile.m_wraps)
                {
                    List<EffectBaseSettingWrap> list = null;
                    EffectPoint key = setting.m_effectPoint;
                    if(!m_activeEffectsDic.TryGetValue(key, out list))
                    {
                        list = new List<EffectBaseSettingWrap>();
                        m_activeEffectsDic.Add(key, list);
                    }

                    list.Add(setting);
                    EffectRendererBase renderer = null;
                    if (!m_activeRendererDic.TryGetValue(setting.m_rendererType, out renderer))
                    {
                        m_activeRendererDic.Add(setting.m_rendererType, Activator.CreateInstance(setting.m_rendererType) as EffectRendererBase);
                    }
                }
            }

            foreach(var effects in m_activeEffectsDic)
            {
                effects.Value.Sort((ea, eb)=> {
                    return ea.m_order - ea.m_order;
                });
            }

            PostProcessEffectMgr.Instance.RegisterLayer(this);
        }

        private void Update()
        {
            foreach(var effects in m_activeEffectsDic)
            {
                foreach(var effect in effects.Value)
                {
                    if(m_activeRendererDic.ContainsKey(effect.m_rendererType))
                    {
                        m_activeRendererDic[effect.m_rendererType].Update();
                    }
                }
            }
        }

        private void OnPreCull()
        {
            SetContext();

            RenderEffectPoint(EffectPoint.BeforeTransparent);
            RenderEffectPoint(EffectPoint.BeforeFinal);
        }

        private void RenderEffectPoint(EffectPoint effectPoint)
        {
            List<EffectBaseSettingWrap> list = null;
            if(m_activeEffectsDic.TryGetValue(effectPoint, out list) && list.Count > 0)
            {
                SetContextCommandBuffer(effectPoint);
                foreach(var setting in list)
                {
                    if(m_activeRendererDic.ContainsKey(setting.m_rendererType))
                    {
                        m_activeRendererDic[setting.m_rendererType].BaseRender(m_context, setting.m_setting);
                    }
                }
            }
        }

        private void SetContextCommandBuffer(EffectPoint effectPoint)
        {
            switch(effectPoint)
            {
                case EffectPoint.BeforeTransparent:
                    m_context.m_command = m_opaqueCommandBuffer;
                    m_opaqueCommandBuffer.Clear();
                    break;
                case EffectPoint.BeforeFinal:
                    m_context.m_command = m_imageEffectCommandBuffer;
                    m_imageEffectCommandBuffer.Clear();
                    break;
                default:
                    break;
            }
        }

        private void SetContext()
        {
            m_context.Reset();
            m_context.m_camera = m_camera;
            m_context.m_source = new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget);
            m_context.m_target = m_context.m_source;
        }
    }
}
