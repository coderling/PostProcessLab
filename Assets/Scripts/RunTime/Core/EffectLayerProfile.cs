using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace PostProcessLab
{
    public class EffectLayerProfile : ScriptableObject
    {
        public List<EffectBaseSettingWrap> m_wraps = new List<EffectBaseSettingWrap>();

        public bool IsExit(Type ty)
        {
            foreach(var wrap in m_wraps)
            {
                if(wrap.m_setting.GetType() == ty)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
