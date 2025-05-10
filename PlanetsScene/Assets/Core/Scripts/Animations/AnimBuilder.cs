using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Frontier_anim
{
    public abstract class AnimBuilder
    {
        public bool isBuilding = false;
        public event Action<Sequence, Type, GameObject> onEndBuild;

        protected GameObject gameObject;
        protected Dictionary<GameObject, Action> callbackByGM;
        
        public AnimBuilder()
        {
            gameObject = null;
            callbackByGM = new Dictionary<GameObject, Action>();
        }

        public void Build()
        {
            GameObject gm = gameObject;
         
            Sequence sequence = EndBuild();
            sequence.OnComplete(
                () =>
                {
                    callbackByGM[gm]?.Invoke();
                }
            );


            sequence.SetLink(gm);
            sequence.SetAutoKill(false);
            sequence.Pause();

            onEndBuild?.Invoke(sequence, this.GetType(), gameObject);
            isBuilding = false;
        }

        public void Initialize()
        {
            if(!callbackByGM.ContainsKey(gameObject))
                callbackByGM.Add(gameObject, null);

            isBuilding = true;

            InitAnim();
        }

        protected abstract void InitAnim();
        protected abstract Sequence EndBuild();

        public virtual AnimBuilder SetGameObject(GameObject gm) 
        {
            gameObject = gm;
            return this;
        }

        public virtual AnimBuilder SetCallback(Action callback)
        {
            callbackByGM[gameObject] = callback;

            return this;
        }

    }
}