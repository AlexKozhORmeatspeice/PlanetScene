using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public interface IPlanetWindow_ScanerFiled
{
    void SetAlpha(float alpha);
}

public class PlanetWindow_ScanerFiled : MonoBehaviour, IPlanetWindow_ScanerFiled
{
    [SerializeField] private RawImage image;
    [SerializeField] private float maxAlpha = 0.6f;

    public void SetAlpha(float alpha)
    {
        if (image == null)
            return;

        alpha = Mathf.Lerp(0.0f, maxAlpha, alpha);

        Color newColor = new Color(image.color.r, image.color.g, image.color.b, alpha);
        image.DOColor(newColor, 0.3f).SetEase(Ease.OutFlash);
    }
}
