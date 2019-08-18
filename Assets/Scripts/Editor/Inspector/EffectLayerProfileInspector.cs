using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace PostProcessLab.Editor
{
    [CustomEditor(typeof(EffectLayerProfile))]
    public class EffectLayerProfileInspector : UnityEditor.Editor
    {
        EffectLayerProfile m_target;
        SerializedProperty m_effectSettingWrapList;

        private void OnEnable()
        {
            m_target = (EffectLayerProfile)target;
            m_effectSettingWrapList = serializedObject.FindProperty("m_wraps");
        }


        Vector2 scroll;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            scroll = GUILayout.BeginScrollView(scroll);
            for(int i = 0; i < m_effectSettingWrapList.arraySize; ++i)
            {
                var effect_wrap = m_effectSettingWrapList.GetArrayElementAtIndex(i);
                if (GUILayout.Button(string.Format("Remove:{0}", effect_wrap.name)))
                {
                    m_effectSettingWrapList.DeleteArrayElementAtIndex(i);
                    RemoveEffect(i);
                    break;
                }
            }

            GUILayout.EndScrollView();
            
            if(GUILayout.Button("Add Effect"))
            {
                var menu = new GenericMenu();
                foreach(var settingInfo in PostProcessEffectMgr.Instance.m_settingAttrs)
                {
                    GUIContent m = new GUIContent(settingInfo.Key.Name);
                    if(m_target.IsExit(settingInfo.Key))
                    {
                        menu.AddDisabledItem(m, true);
                    }
                    else
                    {
                        menu.AddItem(m, false, ()=> AddEffect(settingInfo.Key));
                    }

                }

                menu.ShowAsContext();
            }
        }

        private void AddEffect(Type ty)
        {
            serializedObject.Update();
            var effect_wrap = ScriptableObject.CreateInstance<EffectBaseSettingWrap>();
            //effect_wrap.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            effect_wrap.m_active = true;
           
            //add setting
            effect_wrap.m_setting = (EffectBaseSetting)ScriptableObject.CreateInstance(ty);
            //effect_wrap.m_setting.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            m_effectSettingWrapList.arraySize++;
            var prop = m_effectSettingWrapList.GetArrayElementAtIndex(m_effectSettingWrapList.arraySize - 1);
            if(EditorUtility.IsPersistent(m_target))
            {
                AssetDatabase.AddObjectToAsset(effect_wrap, m_target);
                AssetDatabase.AddObjectToAsset(effect_wrap.m_setting, m_target);
            }
            prop.objectReferenceValue = effect_wrap;

            serializedObject.ApplyModifiedProperties();

            if(EditorUtility.IsPersistent(m_target))
            {
                EditorUtility.SetDirty(m_target);
                AssetDatabase.SaveAssets();
            }
        }

        private void RemoveEffect(int index)
        {
            serializedObject.Update();
            var prop = m_effectSettingWrapList.GetArrayElementAtIndex(index);
            if(prop != null)
            {
                var effect = prop.objectReferenceValue as EffectBaseSettingWrap;

                //必须置为null，否则DeleteArrayElementAtIndex不会使得prop的arraySize减一
                prop.objectReferenceValue = null;

                m_effectSettingWrapList.DeleteArrayElementAtIndex(index);
                serializedObject.ApplyModifiedProperties();


                if(effect != null)
                {
                    if(effect.m_setting != null)
                    {
                        DestroyImmediate(effect.m_setting, true);
                    }
                    DestroyImmediate(effect, true);
                }

                EditorUtility.SetDirty(m_target);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
