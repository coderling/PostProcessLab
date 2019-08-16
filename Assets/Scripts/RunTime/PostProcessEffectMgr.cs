using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PostProcessLab
{
    public sealed class PostProcessEffectMgr
    {
        static PostProcessEffectMgr _Instance;
        public static PostProcessEffectMgr Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = new PostProcessEffectMgr();
                }

                return _Instance;
            }
        }

        private PostProcessEffectMgr()
        {
            m_layerDic = new Dictionary<Camera, EffectLayer>();
#if UNITY_EDITOR
            InitLoadAttriTypes();
#endif
        }

        private Dictionary<Camera, EffectLayer> m_layerDic;

        internal void RegisterLayer(EffectLayer layer)
        {
            if (layer != null)
            {
                var cam = layer.GetComponent<Camera>();
                if(cam != null && !m_layerDic.ContainsKey(cam))
                {
                    m_layerDic.Add(cam, layer);
                }
            }
        }


#if UNITY_EDITOR
        public Dictionary<Type, EffectSettingAttribute> m_settingAttrs;
        
        private void InitLoadAttriTypes()
        {
            m_settingAttrs = new Dictionary<Type, EffectSettingAttribute>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach(var ty in types)
                {
                    if(ty.IsSubclassOf(typeof(EffectBaseSetting)) && ty.IsDefined(typeof(EffectSettingAttribute), false)
                        && !ty.IsAbstract)
                    {
                        m_settingAttrs.Add(ty, ty.GetAttribute<EffectSettingAttribute>());
                    }
                }
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        static void OnReloadScripts()
        {
            Instance.InitLoadAttriTypes();
        }
#endif
    }
}
