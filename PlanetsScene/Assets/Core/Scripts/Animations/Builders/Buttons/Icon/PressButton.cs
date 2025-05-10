using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class PressButton : AnimBuilder
    {
        private Image image;
        private Color color = new Color(0.478f, 0.631f, 0.705f);
        private float scale = 0.95f;
        private float playTime = 0.1f;

        protected override void InitAnim()
        {
            image = gameObject.GetComponent<Image>();
        }

        protected override Sequence EndBuild()
        {
            Sequence hoverSeq = DOTween.Sequence();

            hoverSeq.Insert(0.0f, image.DOColor(color, playTime));
            hoverSeq.Insert(0.0f, gameObject.transform.DOScale(scale, playTime));

            return hoverSeq;
        }

        public PressButton SetImage(Image image)
        {
            this.image = image;
            return this;
        }

        public PressButton SetScale(float scale)
        {
            this.scale = scale;
            return this;
        }

        public PressButton SetColor(Color color)
        {
            this.color = color;
            return this;
        }
    }

}
