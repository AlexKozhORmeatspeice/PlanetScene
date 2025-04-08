using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

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
        infoObj.SetActive(isVisible);
    }


    public void SetGraphicVisibility(bool isVisible)
    {
        if(isVisible)
        {
            graphic.Enable();
            graphicWindowObj.SetActive(true);
        }
        else
        {
            graphicWindowObj.SetActive(false);
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

    public void SetWindowVisabitility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
}
