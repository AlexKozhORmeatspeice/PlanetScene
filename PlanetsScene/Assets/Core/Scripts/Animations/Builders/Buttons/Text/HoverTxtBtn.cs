using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class HoverTxtBtn : AnimBuilder
    {
        private TMP_Text text;
        private Image image;
        private Color color = new Color(0.796f, 0.992f, 1.0f);
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

        public HoverTxtBtn SetImage(Image image)
        {
            this.image = image;
            return this;
        }

        public HoverTxtBtn SetText(TMP_Text text)
        {
            this.text = text;
            return this;
        }

        public HoverTxtBtn SetScale(float scale)
        {
            this.scale = scale;
            return this;
        }

        public HoverTxtBtn SetColor(Color color)
        {
            this.color = color;
            return this;
        }
    }

}
