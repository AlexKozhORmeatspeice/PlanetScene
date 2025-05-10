using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;


namespace Planet_Window
{
    public interface IPointOfInterest
    {
        void SetInfo(PointInfo info);
        
        void SetVisibility(bool visibility);
        void Hide();
        void Show();
        void ChangeScale(float t01);
        void ChangeScale(bool isMax);
        void SetIcon(Texture texture);
        void SetScanerDetection(bool scanerDetection);

        void SetPosition(Vector3 position);

        string Name { get; }
        Vector3 Position { get; }
        Vector3 Size { get; }

        bool IsVisible { get; }
        bool CanScanerSee { get; }

        PointInfo GetInfo { get; }

        GameObject GameObject { get; }
        void PlayActivationAnim();
    }

    public class PointOfInterest : MonoBehaviour, IPointOfInterest, IScreenObject //TODO: объединить с POIInteraction
    {
        [Header("Size")]
        [SerializeField] private Vector2 minSize;
        [SerializeField] private Vector2 maxSize;

        [Header("Objs")]
        [SerializeField] private RawImage icon;
        [SerializeField] private RawImage bg1;
        [SerializeField] private RawImage bg2;
        [SerializeField] private RectTransform rectTransform;

        [Header("Color")]
        [SerializeField] private Color closedBgColor;
        [SerializeField] private Color openBgColor;

        [SerializeField] private Color closedIconColor;
        [SerializeField] private Color openIconColor;

        [SerializeField] private BoxCollider2D collider2d;

        private PointInfo info;
        private bool isPlayingAnim;

        public bool IsVisible => bg1.gameObject.activeSelf;

        public GameObject GameObject => gameObject;

        public string Name => info.nameOfPoint;

        public Vector3 Position => gameObject.transform.position;

        public Vector3 Size => gameObject.transform.localScale;

        public PointInfo GetInfo => info;

        public bool CanScanerSee => collider2d.enabled;

        public void SetInfo(PointInfo info)
        {
            this.info = info;

            SetIcon(info.icon);
            SetVisibility(info.isVisible);
        }

        public void SetPosition(Vector3 position)
        {
            gameObject.transform.position = position;
        }

        public void SetScanerDetection(bool scanerDetection)
        {
            collider2d.enabled = scanerDetection;
        }

        public void SetVisibility(bool visibility)
        {
            if(visibility)
            {
                Show();
            }    
            else
            {
                Hide();
            }
        }

        public void Show()
        {
            info.isVisible = true;
            bg1.gameObject.SetActive(true);
        }

        public void Hide()
        {
            bg1.gameObject.SetActive(false);
        }

        public void SetIcon(Texture texture)
        {
            if (texture == null)
                return;

            icon.texture = texture;
        }

        public void ChangeScale(float t01)
        {
            if (rectTransform == null || isPlayingAnim)
                return;

            t01 = Mathf.Clamp01(t01);

            rectTransform.sizeDelta = maxSize * t01 + minSize * (1.0f - t01);

            SetColors(t01);
        }

        public void ChangeScale(bool isMax)
        {
            ChangeScale(isMax ? 1.0f : 0.0f);
        }

        public void PlayActivationAnim()
        {
            isPlayingAnim = true;

            SetColors(1f);
            rectTransform.sizeDelta = Vector2.zero;
            SetVisibility(true);

            rectTransform.DOSizeDelta(maxSize, 0.35f).SetEase(Ease.OutBounce).
                OnComplete(() =>
                {
                    isPlayingAnim = false;
                });
        }
        private void SetColors(float alpha)
        {
            bg1.color = Color.Lerp(closedBgColor, openBgColor, alpha);
            bg2.color = Color.Lerp(closedBgColor, openBgColor, alpha);
            icon.color = Color.Lerp(closedIconColor, openIconColor, alpha);
        }
    }
}