using System;
using System.Collections;
using System.Collections.Generic;
using Frontier_anim;
using UnityEngine;
using UnityEngine.UI;

namespace Settings_Screen
{

    public interface ISettingsView
    {
        event Action onEndOpenAnim;
        event Action onEndCloseAnim;

        void AnimateShow();
        void AnimateHide();

        void SetVisibility(bool isVisible);
    }

    public class SettingsView : MonoBehaviour, ISettingsView
    {
        [Header("Objs")]
        [SerializeField] private Image bg;
        [SerializeField] private CanvasGroup infoCanvasGroup;
        [SerializeField] private CanvasGroup topBarCanvasGroup;

        public event Action onEndOpenAnim;
        public event Action onEndCloseAnim;

        public void AnimateShow()
        {
            AnimService.Instance.PlayAnim<OpenPopUp>(gameObject);
        }

        public void AnimateHide()
        {
            AnimService.Instance.PlayAnim<ClosePopUp>(gameObject);
        }

        private void Awake()
        {
            AnimService.Instance.BuildAnim<OpenPopUp>(gameObject)
                .SetImage(bg)
                .AddCanvasGroup(infoCanvasGroup)
                .AddCanvasGroup(topBarCanvasGroup, 0.1f, 0.0f, 1.0f, 1.0f, 1.0f)
                .SetCallback(CallEndOpen)
                .Build();

            AnimService.Instance.BuildAnim<ClosePopUp>(gameObject)
                .SetImage(bg)
                .AddCanvasGroup(infoCanvasGroup)
                .AddCanvasGroup(topBarCanvasGroup, 0.1f, 1, 0, 1, 1)
                .SetCallback(CallEndClose)
                .Build();
        }

        private void CallEndOpen()
        {
            onEndOpenAnim?.Invoke();
        }

        private void CallEndClose()
        {
            onEndCloseAnim?.Invoke();
        }

        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }

}
