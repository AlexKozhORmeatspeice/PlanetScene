using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class HoverIconButton : AnimBuilder
    {
        private Image image;
        private Color color = new Color(0.796f, 0.992f, 1.0f);
        private float scale = 1.0f;
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

        public HoverIconButton SetImage(Image image)
        {
            this.image = image;
            return this;
        }

        public HoverIconButton SetScale(float scale)
        {
            this.scale = scale;
            return this;
        }

        public HoverIconButton SetColor(Color color)
        {
            this.color = color;
            return this;
        }
    }

}
