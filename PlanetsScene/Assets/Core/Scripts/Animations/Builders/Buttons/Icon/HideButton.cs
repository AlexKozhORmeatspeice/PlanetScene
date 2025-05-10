using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class HideButton : AnimBuilder
    {
        private Image image;
        private float playTime = 0.1f;
        protected override Sequence EndBuild()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(image.DOFade(0.0f, playTime));

            return sequence;
        }

        protected override void InitAnim()
        {
            image = gameObject.GetComponent<Image>();
        }

        public HideButton SetImage(Image canvasGroup, float playTime = 0.1f)
        {
            this.image = canvasGroup;
            this.playTime = playTime;

            return this;
        }
    }
}