
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Load_screen
{
    public interface IPulsObj
    {
        void StartAnim();
    }

    public class PulsObj : MonoBehaviour, IPulsObj
    {
        [SerializeField] private Image bg;
        [SerializeField] private float timeCycle;
        [SerializeField] private Color firstColor;
        [SerializeField] private Color secondColor;
        private Sequence seq;
        private bool canPlayAnim = false;

        public void StartAnim()
        {
            if (seq == null)
                return;
            
            canPlayAnim = true;
            SetNewAlphaByTime(Time.time);

            //seq.Play();
        }

        private void Update()
        {
            if (!canPlayAnim)
                return;

            SetNewAlphaByTime(Time.time);
        }

        private void Awake()
        {
            seq = DOTween.Sequence();
            seq.Append(bg.DOColor(secondColor, timeCycle / 2.0f).SetEase(Ease.InSine));
            seq.Append(bg.DOColor(firstColor, timeCycle / 2.0f).SetEase(Ease.OutSine));

            seq.SetLoops(-1, LoopType.Yoyo);

            seq.SetLink(gameObject);
            seq.Pause();
        }

        private void SetNewAlphaByTime(float time)
        {
            float t = Mathf.Sin(time * 2 * Mathf.PI / timeCycle) * 0.5f + 0.5f;

            Color newColor = firstColor * t + secondColor * (1.0f - t);
            bg.color = newColor;
        }
    }
}

