using DG.Tweening;
using Load_screen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class SetBasePlanet : AnimBuilder
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

            sequence.Insert(0.0f, rectTransform.DOSizeDelta(closeSize, playTime));
            
            sequence.Insert(0.0f, nameData.outline.DOFade(0.0f, playTime));
            sequence.Insert(0.0f, nameData.bg.DOFade(0.0f, playTime));
            sequence.Insert(0.0f, nameData.text.DOColor(nameData.baseColor, playTime));

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

        public SetBasePlanet SetRect(RectTransform rect, Vector2 _openSize, Vector2 _closeSize)
        {
            rectTransform = rect;
            openSize = _openSize;
            closeSize = _closeSize;

            return this;
        }

        public SetBasePlanet SetButtonIcon(Image icon)
        {
            btnBg = icon;
            return this;
        }

        public SetBasePlanet SetNameData(Image outline, Image bg, TMP_Text text, Color baseColor = default, Color hoveColor = default)
        {
            nameData.outline = outline;
            nameData.bg = bg;
            nameData.text = text;

            nameData.baseColor = baseColor == default ? new Color(0.65f, 0.65f, 0.65f) : baseColor;
            nameData.hoverColor = hoveColor == default ? new Color(1.0f, 1.0f, 1.0f) : hoveColor;

            return this;
        }

        public SetBasePlanet SetBtnBg(Image bg)
        {
            btnBg = bg;
            return this;
        }

        public SetBasePlanet SetLinesCanvasGroup(CanvasGroup lines)
        {
            linesCanvasGroup = lines;
            return this;
        }

        public SetBasePlanet SetBackgroundImage(Image _bg)
        {
            bg = _bg;
            return this;
        }

        public SetBasePlanet SetGlowBg(Image _bg)
        {
            glowBg = _bg;
            return this;
        }
    }
}