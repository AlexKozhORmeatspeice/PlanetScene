using DG.Tweening;
using UnityEngine;

namespace Frontier_anim
{
    public class ShowTxtBtn : AnimBuilder
    {
        private CanvasGroup canvasGroup;
        private float playTime = 0.1f;
        protected override Sequence EndBuild()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(canvasGroup.DOFade(1.0f, playTime));

            return sequence;
        }

        protected override void InitAnim()
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }

        public ShowTxtBtn SetCanvasGroup(CanvasGroup canvasGroup, float playTime = 0.1f)
        {
            this.canvasGroup = canvasGroup;
            this.playTime = playTime;

            return this;
        }
    }
}