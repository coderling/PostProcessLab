using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PostProcessLab
{
    public abstract class EffectRenderer<T> : EffectRendererBase where T : EffectBaseSetting
    {
        public virtual void Init()
        {
        }

        public virtual void Release()
        {
        }

        public virtual void ReStart()
        {
        }

        public virtual void Update()
        {

        }
        
        public virtual void Render(RenderContext context, T setting)
        {

        }


        public void BaseRender(RenderContext context, EffectBaseSetting setting)
        {
            Assert.IsNotNull(setting as T, string.Format("error setting type while need {0}", typeof(T).ToString()));            
            Render(context, setting as T);    
        }
    }

    public interface EffectRendererBase
    {
        void BaseRender(RenderContext context, EffectBaseSetting setting);
        void Update();
    }
}
