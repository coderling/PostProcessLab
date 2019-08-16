using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PostProcessLab
{
    public abstract class EffectRenderer<T> : EffectRendererBase where T : EffectBaseSetting
    {
        public virtual void Render(RenderContext context, T setting)
        {

        }

        public override void BaseRender(RenderContext context, EffectBaseSetting setting)
        {
#if UNITY_EDITOR && EABLE_DEBUG
            Assert.IsNotNull(setting as T, string.Format("error setting type while need {0}", typeof(T).ToString()));            
#endif
            Render(context, setting as T);    
        }
    }

    public abstract class EffectRendererBase
    {
        public virtual void Init()
        {
        }

        public virtual void ReStart()
        {
        }

        public virtual void Update()
        {

        }

        public abstract void BaseRender(RenderContext context, EffectBaseSetting setting);

        public virtual void Release()
        {
        }
    }
}
