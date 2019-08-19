using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.Assertions;

namespace PostProcessLab
{
    internal static class RunTimeHelper
    {
        private static Mesh _fullscreenTriangle;
        private static Mesh FullscreenTriangle
        {
            get
            {
                if(_fullscreenTriangle == null)
                {
                    _fullscreenTriangle = new Mesh { name = "Post Fullscreen tri" };
                    //在shader Vert中会对三角面片进行直接投影，转化为cvv坐标，计算纹理坐标
                    _fullscreenTriangle.SetVertices(new List<Vector3>() {
                        new Vector3(-1f, -1f, 0f),
                        new Vector3(-1f,  3f, 0f),
                        new Vector3( 3f, -1f, 0f)
                    });

                    _fullscreenTriangle.SetIndices(new[] { 0, 1, 2}, MeshTopology.Triangles, 0, false);
                    _fullscreenTriangle.UploadMeshData(false);
                }

                return _fullscreenTriangle;
            }
        }

        private static Material _copyMat;
        private static Material CopyMat
        {
            get
            {
                if(_copyMat == null)
                {
                    _copyMat = new Material(Shader.Find("Hidden/PostProcessLab/Copy"));
                }

                return _copyMat;
            }
        }

        internal static T GetAttribute<T>(this Type ty) where T : Attribute
        {
            Assert.IsTrue(ty.IsDefined(typeof(EffectSettingAttribute), false), string.Format("{0} Attribute not found", ty)); 
            return (T)ty.GetCustomAttributes(typeof(T), false)[0];
        }

        private static int m_rt_id = 0;
        internal static int GetTemporaryRTID()
        {
            return Shader.PropertyToID(string.Format("_PostTarget_{0}", ++m_rt_id));
        }

        internal static void ClearTemporaryRTID()
        {
            m_rt_id = 0;
        }

        internal static void PostBlit(this CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier target)
        {
            //优化blit，target 激活是忽略对其内容的操作
            cmd.SetRenderTarget(target, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            target = BuiltinRenderTextureType.CurrentActive;
            cmd.Blit(source, target, CopyMat, 0);
        }

        internal static void PostBlit(this CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier target, Material mat, int pass = 0)
        {
            //优化blit，target 激活是忽略对其内容的操作
            cmd.SetRenderTarget(target, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            target = BuiltinRenderTextureType.CurrentActive;
            cmd.Blit(source, target, mat, pass);
        }

        internal static void PostBlitFullScreen(this CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier target, Material mat, int pass = 0, RenderBufferLoadAction loadAction = RenderBufferLoadAction.DontCare)
        {
            bool clear = false;
            clear = loadAction == RenderBufferLoadAction.Clear;
            cmd.SetRenderTarget(target, loadAction, RenderBufferStoreAction.Store);
            if(clear)
            {
                cmd.ClearRenderTarget(true, true, Color.clear);
            }

            cmd.SetGlobalTexture(ShaderIDs.ID_MainTex, source);
            cmd.DrawMesh(FullscreenTriangle, Matrix4x4.identity, mat, 0, 0);
        }
    }
}
