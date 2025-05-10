using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class SetBaseTxtBtn : AnimBuilder
    {
        private Image image;
        private Color color = new Color(1.0f, 1.0f, 1.0f);
        private TMP_Text text;
        private float scale = 1.0f;
        private float playTime = 0.1f;

        protected override void InitAnim()
        {
            image = gameObject.GetComponentInChildren<Image>();
            text = gameObject.GetComponentInChildren<TMP_Text>();
        }

        protected override Sequence EndBuild()
        {
            Sequence hoverSeq = DOTween.Sequence();

            hoverSeq.Insert(0.0f, image.DOColor(color, playTime));
            hoverSeq.Insert(0.0f, text.DOColor(color, playTime));
            
            hoverSeq.Insert(0.0f, gameObject.transform.DOScale(scale, playTime));

            return hoverSeq;
        }

        public SetBaseTxtBtn SetImage(Image image)
        {
            this.image = image;
            return this;
        }

        public SetBaseTxtBtn SetText(TMP_Text text)
        {
            this.text = text;
            return this;
        }

        public SetBaseTxtBtn SetScale(float scale)
        {
            this.scale = scale;
            return this;
        }

        public SetBaseTxtBtn SetColor(Color color)
        {
            this.color = color;
            return this;
        }
    }

}
