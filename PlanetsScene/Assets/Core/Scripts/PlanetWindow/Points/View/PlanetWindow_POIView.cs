using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public interface IPlanetWindow_POIView : IGradientChangable
{
    void SetIcon(Texture texture);
    void PlayActivationAnim();

}
public class PlanetWindow_POIView : MonoBehaviour, IPlanetWindow_POIView
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

    private bool isPlayingAnim;

    public string Name { set {} }

    public void SetIcon(Texture texture)
    {
        icon.texture = texture;
    }

    public void SetVisibility(float alpha)
    {
        if (rectTransform == null || isPlayingAnim)
            return;

        alpha = Mathf.Clamp01(alpha);

        rectTransform.sizeDelta = maxSize * alpha + minSize * (1.0f - alpha);

        SetColors(alpha);
    }

    public void SetVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
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
