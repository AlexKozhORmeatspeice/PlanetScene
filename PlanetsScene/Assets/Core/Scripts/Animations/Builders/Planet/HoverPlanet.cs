using DG.Tweening;
using Load_screen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class HoverPlanet : AnimBuilder
    {
        struct NameData
        {
            public Image bg;
            public Image outline;
            public TMP_Text text;
            public Color baseColor;
            public Color hoverColor;
        }

        private float playTime = 0.1f;

        private RectTransform rectTransform;
        private Vector2 closeSize;
        private Vector2 openSize;

        private Image btnBg;

        private Image bg;

        private Image glowBg;

        private CanvasGroup linesCanvasGroup;

        private NameData nameData;

        protected override Sequence EndBuild()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Insert(0.0f, rectTransform.DOSizeDelta(openSize, playTime));

            sequence.Insert(0.0f, nameData.outline.DOFade(0.0f, playTime));
            sequence.Insert(0.0f, nameData.bg.DOFade(1.0f, playTime));
            sequence.Insert(0.0f, nameData.text.DOColor(nameData.hoverColor, playTime));

            sequence.Insert(0.0f, linesCanvasGroup.DOFade(0.0f, playTime));
            sequence.Insert(0.0f, btnBg.DOFade(0.0f, playTime));
            sequence.Insert(0.0f, bg.DOFade(0.0f, playTime));

            sequence.Insert(0.0f, glowBg.DOFade(0.0f, playTime));

            return sequence;
        }

        protected override void InitAnim()
        {
            //
        }

        public HoverPlanet SetRect(RectTransform rect, Vector2 _openSize, Vector2 _closeSize)
        {
            rectTransform = rect;
            openSize = _openSize;
            closeSize = _closeSize;

            return this;
        }

        public HoverPlanet SetButtonIcon(Image icon)
        {
            btnBg = icon;
            return this;
        }

        public HoverPlanet SetNameData(Image outline, Image bg, TMP_Text text, Color baseColor = default, Color hoveColor = default)
        {
            nameData.outline = outline;
            nameData.bg = bg;
            nameData.text = text;

            nameData.baseColor = baseColor == default ? new Color(0.65f, 0.65f, 0.65f) : baseColor;
            nameData.hoverColor = hoveColor == default ? new Color(1.0f, 1.0f, 1.0f) : hoveColor;

            return this;
        }

        public HoverPlanet SetBtnBg(Image bg)
        {
            btnBg = bg;
            return this;
        }

        public HoverPlanet SetLinesCanvasGroup(CanvasGroup lines)
        {
            linesCanvasGroup = lines;
            return this;
        }

        public HoverPlanet SetBackgroundImage(Image _bg)
        {
            bg = _bg;
            return this;
        }

        public HoverPlanet SetGlowBg(Image _bg)
        {
            glowBg = _bg;
            return this;
        }
    }
}