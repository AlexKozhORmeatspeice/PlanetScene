using System;
using System.Collections;
using System.Collections.Generic;
using Frontier_anim;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game_UI
{
    public interface ITextPopUp
    {
        public event Action onEndCloseAnim;

        event UnityAction onCancel;
        event UnityAction onAccept;

        ITextButton CancelBtnView { get; }
        ITextButton AcceptBtnView { get; }

        void SetVisibility(bool isVisible);

        void AnimateOpen();
        void AnimateClose();

        void AnimateHoverAccept();
        void AnimatePressAccept();
        void AnimateBaseAccept();

        void AnimateHoverCancel();
        void AnimatePressCancel();
        void AnimateBaseCancel();

    }

    public class TextPopUp : MonoBehaviour, ITextPopUp
    {
        [SerializeField] private Image image;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextButton cancelButton;
        [SerializeField] private TextButton acceptButton;

        public ITextButton CancelBtnView => cancelButton;
        public ITextButton AcceptBtnView => acceptButton;


        public event Action onEndCloseAnim;

        public event UnityAction onCancel
        {
            add => cancelButton.onClick += value;
            remove => cancelButton.onClick -= value;
        }

        public event UnityAction onAccept
        {
            add => acceptButton.onClick += value;
            remove => acceptButton.onClick -= value;
        }

        public void AnimateOpen()
        {
            AnimService.Instance.PlayAnim<OpenPopUp>(gameObject);
        }

        public void AnimateClose()
        {
            AnimService.Instance.PlayAnim<ClosePopUp>(gameObject);
        }

        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        private void Awake()
        {
            AnimService.Instance.BuildAnim<OpenPopUp>(gameObject)
                .SetImage(image)
                .AddCanvasGroup(canvasGroup)
                .Build();

            AnimService.Instance.BuildAnim<ClosePopUp>(gameObject)
                .SetImage(image)
                .AddCanvasGroup(canvasGroup)
                .SetCallback(InvokeCloseEnd)
                .Build();
        }

        private void InvokeCloseEnd()
        {
            onEndCloseAnim?.Invoke();
        }

        public void AnimateHoverAccept()
        {
            acceptButton.AnimateHover();
        }

        public void AnimatePressAccept()
        {
            acceptButton.AnimatePressed();
        }

        public void AnimateBaseAccept()
        {
            acceptButton.AnimateBaseState();
        }

        public void AnimateHoverCancel()
        {
            cancelButton.AnimateHover();
        }

        public void AnimatePressCancel()
        {
            cancelButton.AnimatePressed();
        }

        public void AnimateBaseCancel()
        {
            cancelButton.AnimateBaseState();
        }
    }

}
