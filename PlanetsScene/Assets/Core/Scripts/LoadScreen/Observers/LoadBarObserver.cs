using Cysharp.Threading.Tasks;
using Game_managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Save_screen
{
    public interface ILoadBarObserver
    {
        void Enable();
        void Disable();
    }


    public class LoadBarObserver : ILoadBarObserver
    {
        [Inject] private ILoadManager loadManager;

        private ILoadBar view;
        AsyncOperation loadOper;
        public LoadBarObserver(ILoadBar view)
        {
            this.view = view;
        }

        public async void Enable()
        {
            loadManager.onChangeProgress += ChangeBarValue;
        }

        public void Disable()
        {
            loadManager.onChangeProgress -= ChangeBarValue;
        }


        private void ChangeBarValue(float value)
        {   
            view.ChangeNormValue(value); 
        }
    }
}
