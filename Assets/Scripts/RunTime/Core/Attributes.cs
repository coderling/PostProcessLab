using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PostProcessLab
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EffectSettingAttribute : Attribute
    {
        public Type m_renderType;
        public EffectPoint m_effectPoint;

        public EffectSettingAttribute(Type renderTye, EffectPoint effectPoint)
        {
            m_renderType = renderTye;
            m_effectPoint = effectPoint;
        }
    }
}
