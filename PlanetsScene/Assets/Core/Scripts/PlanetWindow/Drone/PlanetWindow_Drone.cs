using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public interface IPlanetWindow_Drone
{
    List<Vector3> Trajectory { set; }
    void Init(IPlanetWindow_DronePresenter presenter);
    void PlayAnim();
}

public class PlanetWindow_Drone : MonoBehaviour, IPlanetWindow_Drone
{
    [Header("Sizes")]
    [SerializeField] private Vector2 baseSize;
    [SerializeField] private Vector2 maxSize;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RawImage rawImage;

    private IPlanetWindow_DronePresenter presenter;

    private List<Vector3> trajectory;
    public List<Vector3> Trajectory { set => trajectory = value; }

    public void Init(IPlanetWindow_DronePresenter presenter)
    {
        this.presenter = presenter;
    }

    public void PlayAnim()
    {
        if (trajectory.Count < 3)
            return;

        Color baseColor = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 1.0f);
        Color endColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0.0f);

        rectTransform.position = trajectory[0];
        rectTransform.sizeDelta = baseSize;
        rawImage.color = baseColor;

        var seq = DOTween.Sequence();
        var seq2 = DOTween.Sequence();
        var seq3 = DOTween.Sequence();

        seq.Append(rectTransform.DOMove(trajectory[1], 1.0f));
        seq2.Append(rectTransform.DOSizeDelta(maxSize, 1.0f));
        seq3.Append(rawImage.DOColor(baseColor, 1.1f));

        seq.Append(rectTransform.DOMove(trajectory[2], 0.3f).SetEase(Ease.InSine));
        seq2.Append(rectTransform.DOSizeDelta(Vector2.zero, 0.3f).SetEase(Ease.InSine));
        seq3.Append(rawImage.DOColor(endColor, 0.2f).SetEase(Ease.InSine));

        seq3.OnComplete(() =>
        {
            presenter.EndAnim();
        });
    }
}
