using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public interface IMenuSideBarView
    {
        void AnimatePosition(Vector3 newPos);
        Vector3 LocalPosition { get; set; }
    }

    public class MenuSideBarView : MonoBehaviour, IMenuSideBarView
    {
        [SerializeField] private float timeSideBarChange = 0.3f;

        public Vector3 LocalPosition
        {
            get => transform.localPosition;
            set
            {
                transform.localPosition = value;
            }
        }

        public void AnimatePosition(Vector3 newPos)
        {
            transform.DOLocalMove(newPos, timeSideBarChange).SetEase(Ease.OutSine);
        }
    }
}


