using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_UI
{
    public interface IStartFadePanel
    {
        event Action onEndHideScreen;
        event Action onEndShowScreen;
        void AnimateHideScreen();
        void AnimateShowScreen();
    }

    public class StartFadePanel : MonoBehaviour, IStartFadePanel
    {
        [SerializeField] private Image image;
        [SerializeField] private float playTime = 0.1f;

        public event Action onEndHideScreen;
        public event Action onEndShowScreen;

        public void AnimateShowScreen()
        {
            image.DOFade(0.0f, playTime)
                .OnComplete(() =>
                {
                    onEndShowScreen?.Invoke();
                });
        }

        public void AnimateHideScreen()
        {
            image.DOFade(1.0f, 0.1f)
                .OnComplete(() =>
                {
                    onEndHideScreen?.Invoke();
                });
        }
    }
}

