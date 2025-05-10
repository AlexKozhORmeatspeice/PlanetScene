using System;
using System.Collections;
using System.Collections.Generic;
using Frontier_anim;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game_UI
{
    public interface ITextInputPopUp
    {
        event UnityAction onCancel;
        event UnityAction onAccept;
        string InputText { get; set; }
        void SetVisibility(bool isVisible);

        event Action onEndCloseAnim;

        ITextButton CancelBtnView { get; }
        ITextButton AcceptBtnView { get; }

        void AnimateOpen();
        void AnimateClose();

        void AnimateHoverAccept();
        void AnimatePressAccept();
        void AnimateBaseAccept();

        void AnimateHoverCancel();
        void AnimatePressCancel();
        void AnimateBaseCancel();
    }

    public class TextInputPopUp : MonoBehaviour, ITextInputPopUp
    {
        [SerializeField] private Image bg;
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private TextButton cancelButton;
        [SerializeField] private TextButton acceptButton;
        [SerializeField] private TMP_InputField inputField;

        public event Action onEndCloseAnim;

        public string InputText
        {
            get
            {
                return inputField.text;
            }
            set
            {
                inputField.text = value;
            }
        }

        public ITextButton CancelBtnView => cancelButton;
        public ITextButton AcceptBtnView => acceptButton;


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
                .SetImage(bg)
                .AddCanvasGroup(canvasGroup)
                .Build();

            AnimService.Instance.BuildAnim<ClosePopUp>(gameObject)
                .SetImage(bg)
                .AddCanvasGroup(canvasGroup)
                .SetCallback(InvokeCloseAction)
                .Build();
        }

        private void InvokeCloseAction()
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

