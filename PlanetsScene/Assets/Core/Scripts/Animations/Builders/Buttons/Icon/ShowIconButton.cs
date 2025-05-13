using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class ShowIconButton : AnimBuilder
    {
        private Image image;
        private float playTime = 0.1f;
        protected override Sequence EndBuild()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(image.DOFade(1.0f, playTime));

            return sequence;
        }

        protected override void InitAnim()
        {
            image = gameObject.GetComponent<Image>();
        }

        public ShowIconButton SetImage(Image canvasGroup, float playTime = 0.1f)
        {
            this.image = canvasGroup;
            this.playTime = playTime;

            return this;
        }
    }
}