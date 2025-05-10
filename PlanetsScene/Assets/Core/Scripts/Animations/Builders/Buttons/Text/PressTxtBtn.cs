using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class PressTxtBtn : AnimBuilder
    {
        private TMP_Text text;
        private Image image;
        private Color color = new Color(0.478f, 0.631f, 0.705f);
        private float scale = 0.95f;
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

        public PressTxtBtn SetImage(Image image)
        {
            this.image = image;
            return this;
        }

        public PressTxtBtn SetText(TMP_Text text)
        {
            this.text = text;
            return this;
        }

        public PressTxtBtn SetScale(float scale)
        {
            this.scale = scale;
            return this;
        }

        public PressTxtBtn SetColor(Color color)
        {
            this.color = color;
            return this;
        }
    }

}
