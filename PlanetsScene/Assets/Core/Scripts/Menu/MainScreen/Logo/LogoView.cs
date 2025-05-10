using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public interface ILogoView
    {
        void AnimateAlpha(float alpha);

        Color ImageColor { get; set; }
    }
    public class LogoView : MonoBehaviour, ILogoView
    {
        [SerializeField] private float timeChangeAlpha = 1.0f;
        [SerializeField] private Ease easeType = Ease.InSine;

        [SerializeField] private Color startColor;
        [SerializeField] private Color endColor;
        [SerializeField] private Image image;

        public Color ImageColor 
        { 
            get => image.color; 
            set => image.color = value; 
        }

        public void AnimateAlpha(float alpha)
        {
            Color endColor = image.color;
            endColor.a = alpha;

            image.DOColor(endColor, timeChangeAlpha).SetEase(easeType);
        }
    }

}