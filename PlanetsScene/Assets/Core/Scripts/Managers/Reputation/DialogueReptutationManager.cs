using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Reptutation_Screen
{
    public interface IReptutationManager
    {
        event Action<int, IFraction> onReptutationChange;
    }

    public class DialogueReptutationManager : MonoBehaviour, IReptutationManager, IStartable
    {
        [Inject] private IFractionSpawner fractionSpawner;

        public event Action<int, IFraction> onReptutationChange;

        public void Initialize()
        {
            
        }

        public void Start() {}

        public void OnSceneChange(string sceneName)
        {

        }
    }
}


