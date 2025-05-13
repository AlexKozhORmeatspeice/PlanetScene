using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Frontier_anim;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace Planet_Window
{
    public interface IPlanetWindowView
    {
        string Name { set; }
        string Title { set; }

        string Description { set; }

        void SetWindowVisabitility(bool isVisible);

        void SetInfoVisibility(bool isVisible);

        void SetGraphicVisibility(bool isVisible);

        void ShowScanerFiled();
        void HideScanerFiled();

        void PlayOpenAnim();
        void PlayCloseAnim();
    }

    public class PlanetWindowView : MonoBehaviour, IPlanetWindowView
    {
        [Header("Graphic")]
        [SerializeField] private GameObject graphicWindowObj;
        [Inject] private IPointOfInterestGraphic graphic;

        [Header("Scaner Filed")]
        [SerializeField] private RawImage image;
        [SerializeField][Range(0.0f, 1.0f)] private float maxAlpha = 0.6f;

        [Header("Info text")]
        [SerializeField] private GameObject infoObj;
        [SerializeField] private TMP_Text sectorTxt;
        [SerializeField] private TMP_Text nameTxt;
        [SerializeField] private TMP_Text descriptionTxt;

        [Header("Anim objs")]
        [SerializeField] private Image bg;
        [SerializeField] private CanvasGroup planetCanvasGroup;
        [SerializeField] private CanvasGroup infoCanvasGroup;
        [SerializeField] private CanvasGroup panelCanvasGroup;
        [SerializeField] private CanvasGroup scanerCanvasGroup;
        [SerializeField] private LineRenderer lineRenderer;

        string IPlanetWindowView.Title
        {
            set
            {
                sectorTxt.text = value;
            }
        }
        string IPlanetWindowView.Name
        {
            set
            {
                nameTxt.text = value;
            }
        }
        string IPlanetWindowView.Description
        {
            set
            {
                descriptionTxt.text = value;
            }
        }

        public void SetInfoVisibility(bool isVisible)
        {
            if (isVisible)
            {
                AnimateInfoShow();
            }
            else
            {
                AnimateInfoHide();
            }
        }


        public void SetGraphicVisibility(bool isVisible)
        {
            if (isVisible)
            {
                graphic.Enable();
                AnimateGraphicShow();
            }
            else
            {
                AnimateGraphicHide();
                graphic.Disable();
            }
        }


        public void ShowScanerFiled()
        {
            SetScanerFiledAlpha(1.0f);
        }

        public void HideScanerFiled()
        {
            SetScanerFiledAlpha(0.0f);
        }

        private void SetScanerFiledAlpha(float alpha)
        {
            if (image == null)
                return;

            alpha = Mathf.Lerp(0.0f, maxAlpha, alpha);

            Color newColor = new Color(image.color.r, image.color.g, image.color.b, alpha);
            image.DOColor(newColor, 0.3f).SetEase(Ease.OutFlash);
        }

        private void AnimateInfoShow()
        {
            infoObj.SetActive(true);

            AnimService.Instance.PlayAnim<OpenPopUp>(infoObj);
        }

        private void AnimateInfoHide()
        {
            AnimService.Instance.PlayAnim<ClosePopUp>(infoObj);
        }

        private void AnimateGraphicHide()
        {
            AnimService.Instance.PlayAnim<ClosePopUp>(graphicWindowObj);
        }

        private void AnimateGraphicShow()
        {
            graphicWindowObj.SetActive(true);

            AnimService.Instance.PlayAnim<OpenPopUp>(graphicWindowObj);
        }

        public void SetWindowVisabitility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        public void PlayOpenAnim()
        {
            AnimService.Instance.PlayAnim<OpenPopUp>(gameObject);
        }

        public void PlayCloseAnim()
        {
            AnimService.Instance.PlayAnim<ClosePopUp>(gameObject);
        }

        public void DisableWindow()
        {
            SetWindowVisabitility(false);
        }

        private void SetUnactiveGraphic()
        {
            graphicWindowObj.SetActive(false);
        }    

        private void SetUnactiveInfo()
        {
            infoObj.SetActive(false);
        }

        public void Awake()
        {
            AnimService.Instance.BuildAnim<OpenPopUp>(gameObject)
                .SetImage(bg, 0.0f, 0.98f)
                .AddCanvasGroup(planetCanvasGroup)
                .AddCanvasGroup(panelCanvasGroup)
                .Build();

            AnimService.Instance.BuildAnim<ClosePopUp>(gameObject)
                .SetImage(bg, 0.0f, 0.98f)
                .AddCanvasGroup(planetCanvasGroup)
                .AddCanvasGroup(panelCanvasGroup)
                .SetCallback(DisableWindow)
                .Build();

            AnimService.Instance.BuildAnim<OpenPopUp>(gameObject)
                .SetImage(bg)
                .AddCanvasGroup(planetCanvasGroup)
                .AddCanvasGroup(panelCanvasGroup)
                .Build();

            AnimService.Instance.BuildAnim<OpenPopUp>(infoObj)
                .AddCanvasGroup(infoCanvasGroup)
                .Build();

            AnimService.Instance.BuildAnim<ClosePopUp>(infoObj)
                .AddCanvasGroup(infoCanvasGroup)
                .SetCallback(SetUnactiveInfo)
                .Build();

            AnimService.Instance.BuildAnim<OpenPopUp>(graphicWindowObj)
                .AddCanvasGroup(scanerCanvasGroup)
                .Build();

            AnimService.Instance.BuildAnim<ClosePopUp>(graphicWindowObj)
                .AddCanvasGroup(scanerCanvasGroup)
                .SetCallback(SetUnactiveGraphic)
                .Build();
        }
    }

}