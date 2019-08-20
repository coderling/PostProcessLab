using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace PostProcessLab
{
    [Serializable]
    public class EffectBaseSetting : ScriptableObject 
    {
        [HideInInspector]
        public int tt = 0;
        public virtual string m_shader
        {
            get;
        }

        private static Material _m_mat;
        public Material m_mat
        {
            get
            {
                if(m_shader != null && _m_mat == null)
                {
                    _m_mat = new Material(Shader.Find(m_shader));
                }
                Assert.IsNotNull(_m_mat);             
                return _m_mat;
            }
        }
    }
}

