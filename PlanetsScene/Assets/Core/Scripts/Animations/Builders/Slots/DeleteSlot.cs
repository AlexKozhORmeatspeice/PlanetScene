using DG.Tweening;
using UnityEngine;

namespace Frontier_anim
{
    public class DeleteSlot : AnimBuilder
    {
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;

        protected override Sequence EndBuild()
        {
            Sequence sequence = DOTween.Sequence();

            Vector2 endDelta = rectTransform.sizeDelta;
            endDelta.y = 0.0f;

            sequence.Append(canvasGroup.DOFade(0.0f, 0.1f).SetEase(Ease.Linear));
            sequence.Append(rectTransform.DOSizeDelta(endDelta, 0.2f).SetEase(Ease.Linear));
            
            return sequence;
        }

        protected override void InitAnim()
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            rectTransform = gameObject.GetComponent<RectTransform>();
        }

        public DeleteSlot SetRectTransform(RectTransform rectTransform)
        {
            this.rectTransform = rectTransform;
            return this;
        }

        public DeleteSlot SetCanvasGroup(CanvasGroup canvasGroup)
        {
            this.canvasGroup = canvasGroup;
            return this;
        }
    }
}