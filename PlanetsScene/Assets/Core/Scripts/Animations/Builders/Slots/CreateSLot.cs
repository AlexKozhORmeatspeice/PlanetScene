using DG.Tweening;
using UnityEngine;

namespace Frontier_anim
{
    public class CreateSlot : AnimBuilder
    {
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        protected override Sequence EndBuild()
        {
            Sequence sequence = DOTween.Sequence();

            Vector2 endDelta = rectTransform.sizeDelta;
            Vector2 startDelta = endDelta;
            startDelta.y = 0.0f;

            sequence.Append(rectTransform.DOSizeDelta(startDelta, 0.0f).SetEase(Ease.Linear));
            sequence.Append(canvasGroup.DOFade(0.0f, 0.0f).SetEase(Ease.Linear));
            sequence.Append(rectTransform.DOSizeDelta(endDelta, 0.2f).SetEase(Ease.Linear));
            sequence.Append(canvasGroup.DOFade(1.0f, 0.1f).SetEase(Ease.Linear));

            return sequence;
        }

        protected override void InitAnim()
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            rectTransform = gameObject.GetComponent<RectTransform>();
        }

        public CreateSlot SetRectTransform(RectTransform rectTransform)
        {
            this.rectTransform = rectTransform;
            return this;
        }

        public CreateSlot SetCanvasGroup(CanvasGroup canvasGroup)
        {
            this.canvasGroup = canvasGroup;
            return this;
        }
    }
}