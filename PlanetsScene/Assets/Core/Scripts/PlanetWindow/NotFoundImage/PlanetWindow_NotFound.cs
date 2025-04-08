using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public interface IPlanetWindow_NotFound
{
    Vector3 Pos { set; }
    void PlayAnim();
    void SetActive(bool active);
}

public class PlanetWindow_NotFound : MonoBehaviour, IPlanetWindow_NotFound
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 size;
    public Vector3 Pos { set => rectTransform.position = value; }

    public void PlayAnim()
    { 
        rectTransform.sizeDelta = Vector2.zero;

        var seq = DOTween.Sequence();
        seq.Append(rectTransform.DOSizeDelta(size, 0.35f).SetEase(Ease.OutBounce));
        seq.Append(rectTransform.DOSizeDelta(size, 0.2f));
        seq.Append(rectTransform.DOSizeDelta(Vector2.zero, 0.35f));
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
