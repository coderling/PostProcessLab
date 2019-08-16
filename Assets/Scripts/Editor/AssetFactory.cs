using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System.IO; 

namespace PostProcessLab.Editor
{
    static class AssetFactory
    {
        const int MenuBasePriority = 200;

        private class DoCreateEffectSettingAsset<T> : EndNameEditAction where T : EffectBaseSettingWrap
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                var newAsset = CreateInstance<T>();
                newAsset.name = Path.GetFileName(pathName);

                AssetDatabase.CreateAsset(newAsset, pathName);
                ProjectWindowUtil.ShowCreatedAsset(newAsset);
            }
        }
        

        private class DoCreateEffectLayerProfileAsset : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                var newAsset = CreateInstance<EffectLayerProfile>();
                newAsset.name = Path.GetFileName(pathName);
                AssetDatabase.CreateAsset(newAsset, pathName);
                ProjectWindowUtil.ShowCreatedAsset(newAsset);
            }
        }

        [MenuItem("Assets/Create/PostEffectLab/EffectLayer Profile", priority = MenuBasePriority)]
        static void CreateEffectLayerProfile()
        {
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateEffectLayerProfileAsset>(), "New EffectLayer Profile.asset", icon, null);
        }
    }
}
