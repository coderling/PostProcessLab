using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.Assertions;

namespace PostProcessLab
{
    public static class RunTimeHelper
    {
        public static T GetAttribute<T>(this Type ty) where T : Attribute
        {
            Assert.IsTrue(ty.IsDefined(typeof(EffectSettingAttribute), false), string.Format("{0} Attribute not found", ty)); 
            return (T)ty.GetCustomAttributes(typeof(T), false)[0];
        }

        public static void PostBlit(this CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier target)
        {
#if UNITY_2018_2_OR_NEWER
            //优化blit，target 激活是忽略对其内容的操作
            cmd.SetRenderTarget(target, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            target = BuiltinRenderTextureType.CurrentActive;
#endif
            cmd.Blit(source, target);
        }

        public static void PostBlit(this CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier target, Material mat, int pass = 0)
        {
#if UNITY_2018_2_OR_NEWER
            //优化blit，target 激活是忽略对其内容的操作
            cmd.SetRenderTarget(target, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            target = BuiltinRenderTextureType.CurrentActive;
#endif
            cmd.Blit(source, target, mat, pass);
        }
    }
}
