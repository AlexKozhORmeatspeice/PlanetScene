
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Load_screen
{
    public interface IPulsObj
    {
        Color GetNowColor();
        void StartAnim();
        void StopAnim();
    }

    public class PulsObj : MonoBehaviour, IPulsObj
    {
        [SerializeField] private Image bg;
        [SerializeField] private float timeCycle;
        [SerializeField] private Color firstColor;
        [SerializeField] private Color secondColor;
        private bool canPlayAnim = false;
        private float startRandTimeOffset;
        public void StartAnim()
        {   
            canPlayAnim = true;
            SetNewAlphaByTime(Time.time);
        }

        public void StopAnim()
        {
            canPlayAnim = false;
        }

        private void Update()
        {
            if (!canPlayAnim)
                return;
            SetNewAlphaByTime(Time.time + startRandTimeOffset);
        }

        private void SetNewAlphaByTime(float time)
        {
            float t = Mathf.Sin(time * 2 * Mathf.PI / timeCycle) * 0.5f + 0.5f;

            Color newColor = firstColor * t + secondColor * (1.0f - t);
            bg.color = newColor;
        }

        public Color GetNowColor()
        {
            float t = Mathf.Sin((Time.time + startRandTimeOffset) * 2 * Mathf.PI / timeCycle) * 0.5f + 0.5f;

            Color newColor = firstColor * t + secondColor * (1.0f - t);

            return newColor;
        }

        private void Awake()
        {
            startRandTimeOffset = UnityEngine.Random.Range(0.0f, timeCycle);
        }
    }
}

