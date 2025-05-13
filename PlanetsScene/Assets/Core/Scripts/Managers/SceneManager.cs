
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_managers
{

    public enum SceneType //названия писать четко в соответветствии с названием сцены в Unity
    {
        LoadScreen,
        Menu,
        Sectors
    }

    public interface ISceneManager
    {
        event Action<SceneType> onStartLoad;
        event Action<float> onChangeProgress;
        event Action<SceneType> onEndLoad;
        UniTask LoadSceneAsync(SceneType sceneType);
    }

    public class SceneManager : ISceneManager
    {
        public event Action<SceneType> onStartLoad;
        public event Action<SceneType> onEndLoad;

        public event Action<float> onChangeProgress;

        private static SceneType activeScene;

        public async UniTask LoadSceneAsync(SceneType sceneType)
        {
            if (activeScene == sceneType)
                return;
            activeScene = sceneType;

            AsyncOperation loadOper = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneType.ToString());
            loadOper.allowSceneActivation = false;
            
            onStartLoad?.Invoke(sceneType);

            //////// TODO: удалить после показа ///////////
            int counts = 100;
            for (int i = 0; i < counts; i += UnityEngine.Random.Range(1, 5))
            {
                onChangeProgress?.Invoke((float)i / counts);
                await UniTask.Delay(200);
            }
            onChangeProgress?.Invoke(1.0f);
            //////////////////////////////////////////////

            /////////// TODO: вернуть после показа /////////////
            /*while (loadOper.progress < 0.9f) //
            {
                onChangeProgress?.Invoke(loadOper.progress);
            }
            onChangeProgress?.Invoke(1.0f);*/
            ////////////////////////////////////////////////////

            if (onEndLoad != null)
            {
                var endLoadTask = UniTask.RunOnThreadPool(() => onEndLoad?.Invoke(sceneType));
                await endLoadTask;
            }

            loadOper.allowSceneActivation = true;
        }
    }
}