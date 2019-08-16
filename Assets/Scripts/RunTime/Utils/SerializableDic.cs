using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace PostProcessLab
{
    [Serializable]
    public class SerializableDic<TK, TV> : Dictionary<TK, TV>, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<TK> m_keys = new List<TK>();

        [SerializeField]
        List<TV> m_values = new List<TV>();

        public void OnAfterDeserialize()
        {
            //Assert.AreEqual(m_keys.Count, m_values.Count);
            for (int i = 0; i < m_keys.Count; ++i)
            {
                this.Add(m_keys[i], m_values[i]); 
            }

            m_keys.Clear();
            m_values.Clear();
        }

        public void OnBeforeSerialize()
        {
            m_keys.Clear();
            m_values.Clear();
            foreach (var va in this)
            {
                m_keys.Add(va.Key);
                m_values.Add(va.Value);
            }
        }
    }
}
