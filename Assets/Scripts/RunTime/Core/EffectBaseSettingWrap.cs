using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PostProcessLab
{
    [Serializable]
    public class EffectBaseSettingWrap : ScriptableObject
    {
        public bool m_active;
        public EffectBaseSetting m_setting;
        public int m_order;
        private EffectSettingAttribute _m_attribute;
        public EffectSettingAttribute m_attribute
        {
            get
            {
                if (_m_attribute == null)
                {
                    _m_attribute = m_setting.GetType().GetAttribute<EffectSettingAttribute>();
                }

                return _m_attribute;
            }
        }

        internal Type m_rendererType
        {
            get
            {
                return m_attribute.m_renderType;
            }
        }

        internal EffectPoint m_effectPoint
        {
            get
            {
                return m_attribute.m_effectPoint;
            }
        }
    }
}
