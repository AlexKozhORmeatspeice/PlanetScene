using System;
using DG.Tweening;
using Frontier_anim;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Save_screen
{
    public interface ISaveScreenView
    {
        event Action onEndOpenAnim;
        event Action onEndCloseAnim;

        void AnimateEnable();
        void AnimateDisable();

        void SetVisibility(bool isVisible);
    }

    public class SaveScreenView : MonoBehaviour, ISaveScreenView
    {
        [Header("Objs")]
        [SerializeField] private Image bg;
        [SerializeField] private CanvasGroup infoCanvasGroup;
        [SerializeField] private CanvasGroup topBarCanvasGroup;

        public event Action onEndOpenAnim;
        public event Action onEndCloseAnim;

        public void AnimateEnable()
        {
            AnimService.Instance.PlayAnim<OpenPopUp>(gameObject);
        }

        public void AnimateDisable()
        {
            AnimService.Instance.PlayAnim<ClosePopUp>(gameObject);
        }

        private void Awake()
        {
            AnimService.Instance.BuildAnim<OpenPopUp>(gameObject)
                .SetImage(bg)
                .AddCanvasGroup(infoCanvasGroup)
                .AddCanvasGroup(topBarCanvasGroup, 0.2f, 0.0f, 1.0f, 1.0f, 1.0f)
                .SetCallback(CallEndOpen)
                .Build();

            AnimService.Instance.BuildAnim<ClosePopUp>(gameObject)
                .SetImage(bg)
                .AddCanvasGroup(infoCanvasGroup)
                .AddCanvasGroup(topBarCanvasGroup, 0.2f, 1, 0, 1, 1)
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