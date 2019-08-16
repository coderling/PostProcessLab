using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using UnityEditor;

namespace PostProcessLab
{
    public static class SerializedPropertyExtend
    {
        ///原函数不支持扩展
        public static SerializedProperty FindPropertyRelativeEx(this SerializedProperty property,string relativePath)
        {
            var prop = property.Copy();
            string fullPath = string.Format("{0}.{1}", prop.propertyPath, relativePath);
            bool enter = true;
            var end = prop.GetEndProperty();
            while (prop.Next(enter))
            {
                if(prop.propertyPath == fullPath)
                {
                    return prop;
                }
                enter = false;
            }
            return null;
        }
    }
}
