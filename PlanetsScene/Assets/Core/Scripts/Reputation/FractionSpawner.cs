using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Reptutation_Screen
{
    [Serializable]
    public struct FractionInfo
    {
        public string Name;
        public Texture icon;
        public int startReputation;
    }

    public interface IFractionSpawner
    {
        event Action <FractionInfo, IFraction> onUpdateInfo;
    }

    public class FractionSpawner : MonoBehaviour, IFractionSpawner, IStartable, IDisposable
    {
        [Inject] private IObjectResolver resolver;

        [Header("UI settings")]
        [SerializeField] private Transform startTransform;
        [SerializeField] private float topOffset = 2.0f;
        [SerializeField] private float distBetween = 5.0f;

        [Header("Objs")]
        [SerializeField] private Transform parentObject;
        [SerializeField] private Fraction fractionPrefab;
        [SerializeField] private List<FractionInfo> fractionInfos;
        
        private List<IFractionObserver> fractionObservers;

        public event Action<FractionInfo, IFraction> onUpdateInfo;

        public void Initialize()
        {
            fractionObservers = new List<IFractionObserver>();

            Vector3 startPos = startTransform.position;
            Vector3 nowPos = startPos;

            float stepY = -(fractionPrefab.Size.y + distBetween);
            foreach (FractionInfo info in fractionInfos)
            {
                IFraction newFraction = GameObject.Instantiate(fractionPrefab, nowPos, Quaternion.identity, parentObject);
                IFractionObserver observer = new FractionObserver(newFraction);
                resolver.Inject(observer);
                observer.Enable();

                onUpdateInfo?.Invoke(info, newFraction);
                nowPos.y += stepY;   
            }
        }

        public void Start() { }

        public void Dispose()
        {
            foreach(IFractionObserver observer in fractionObservers)
            {
                observer.Disable();
            }
        }
    }
}