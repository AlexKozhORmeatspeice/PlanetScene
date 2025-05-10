using System;
using System.Collections.Generic;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Frontier_anim
{
    public enum AnimType
    {
        ClosePopUp,
        OpenPopUp
    }

    public interface IAnimService
    {
        void PlayAnim<T>(GameObject gameObject) where T : AnimBuilder;
        T BuildAnim<T>(GameObject gameObject) where T : AnimBuilder, new();
    }

    public class AnimService : IAnimService
    {
        private static AnimService instance;

        private Dictionary<GameObject, Dictionary<Type, Sequence>> sequenceByGM;
        private Dictionary<Type, AnimBuilder> builderByType;

        AnimBuilder nowActiveBuilder;
        
        public AnimService()
        {
            builderByType = new Dictionary<Type, AnimBuilder>();
            sequenceByGM = new Dictionary<GameObject, Dictionary<Type, Sequence>>();
        }

        public static AnimService Instance
        {
            get
            {
                if (instance == null)
                    instance = new AnimService();

                return instance;
            }
        }

        public T BuildAnim<T>(GameObject gameObject) where T : AnimBuilder, new()
        {
            Type type = typeof(T);
            if(IsGotAnim(type, gameObject))
            {
                sequenceByGM[gameObject][type].Kill();
            }

            if(!builderByType.ContainsKey(type))
            {
                builderByType.Add(type, new T());
            }

            nowActiveBuilder = builderByType[type];
            if (nowActiveBuilder != null && nowActiveBuilder.isBuilding)
            {
                throw new Exception("Builder of type " + nowActiveBuilder.GetType() + " not called Build() method");
            }

            nowActiveBuilder.SetGameObject(gameObject);
            nowActiveBuilder.Initialize();

            nowActiveBuilder.onEndBuild += SetAnim;
            
            return nowActiveBuilder as T;
        }

        private void SetAnim(Sequence sequence, Type animType, GameObject gm)
        {
            if(!IsGotAnim(animType, gm)) //мне немного вообще не нравится, что тут творится с созданием словарей, но лучше я пока не придумал
            {
                if(sequenceByGM.ContainsKey(gm))
                {
                    sequenceByGM[gm].Add(animType, sequence);
                }
                else
                {
                    sequenceByGM.Add(gm, new Dictionary<Type, Sequence>());
                    sequenceByGM[gm].Add(animType, sequence);
                }
            }
            else
            {
                sequenceByGM[gm][animType].Kill();
                sequenceByGM[gm][animType] = sequence;
            }

            nowActiveBuilder.onEndBuild -= SetAnim;
            nowActiveBuilder = null;
        }

        public void PlayAnim<T>(GameObject gameObject) where T: AnimBuilder
        {
            if (gameObject == null)
                return;

            Sequence nowSeq;
            Type type = typeof(T);
            if (!IsGotAnim(type, gameObject))
            {
                Debug.LogError("Animation " + type.ToString() + " was not created for an object " + gameObject.name);
                return;
            }
            
            nowSeq = sequenceByGM[gameObject][type];
            if(nowSeq.IsPlaying())
            {
                nowSeq.Restart();
            }

            nowSeq.Rewind();
            nowSeq.Play();
                        
            return;
        }

        private bool IsGotAnim(Type type, GameObject gameObject)
        {
            return sequenceByGM.ContainsKey(gameObject) && sequenceByGM[gameObject].ContainsKey(type);
        }

    }
}