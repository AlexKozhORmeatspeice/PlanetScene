using System;
using DG.Tweening;
using Frontier_anim;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VContainer;
using VContainer.Unity;

namespace Top_Bar
{
    public interface ITopBarView
    {
        event UnityAction onBackClick;

        void AnimateHideSectorBar();
        void AnimateShowSectorBar();

        void AnimateHideButtonsBar();
        void AnimateShowButtonsBar();

        void AnimateBackBtnHover();
        void AnimateBackBtnBase();
        void AnimateBackBtnPress();

    }

    public class TopBarView : MonoBehaviour, ITopBarView
    {
        [Header("Anim settings")]
        [SerializeField] private float barAnimTime = 0.2f;
        [SerializeField] private float Y_AnimOffsetStart = -0.5f;

        [Header("Objs")]
        [SerializeField] private IconButton goBackBtn;
        [SerializeField] private IconButton encyclopediaBtn;
        [SerializeField] private IconButton playerBtn;
        [SerializeField] private TMP_Text sectorNameTxt;
        [SerializeField] private Transform buttonsBarTrans;
        [SerializeField] private Transform sectorNameBarTrans;

        private bool isSectorHidden;
        private bool isButtonsHidden;

        public event UnityAction onBackClick
        {
            add => goBackBtn.onClick += value;
            remove => goBackBtn.onClick -= value;
        }

        public void AnimateBackBtnBase()
        {
            AnimService.Instance.PlayAnim<SetBaseIconButton>(goBackBtn.gameObject);
        }

        public void AnimateBackBtnHover()
        {
            AnimService.Instance.PlayAnim<HoverIconButton>(goBackBtn.gameObject);
        }

        public void AnimateBackBtnPress()
        {
            AnimService.Instance.PlayAnim<PressIconButton>(goBackBtn.gameObject);
        }

        public void AnimateHideButtonsBar()
        {
            if (isButtonsHidden) return;
            isButtonsHidden = true;

            buttonsBarTrans.DOLocalMoveY(-Y_AnimOffsetStart, barAnimTime);
        }

        public void AnimateHideSectorBar()
        {
            if (isSectorHidden) return;
            isSectorHidden = true;

            sectorNameBarTrans.DOLocalMoveY(-Y_AnimOffsetStart, barAnimTime);
        }

        public void AnimateShowButtonsBar()
        {
            if (!isButtonsHidden) return;
            isButtonsHidden = false;

            buttonsBarTrans.DOLocalMoveY(Y_AnimOffsetStart, barAnimTime);
        }

        public void AnimateShowSectorBar()
        {
            if (!isSectorHidden) return;
            isSectorHidden = false;

            sectorNameBarTrans.DOLocalMoveY(Y_AnimOffsetStart, barAnimTime);
        }

        private void Awake()
        {
            isSectorHidden = true;
            isButtonsHidden = true;
        }
    }
}
