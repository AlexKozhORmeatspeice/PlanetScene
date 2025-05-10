using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Frontier_anim;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public interface IIconButton
{
    event UnityAction onClick;

    Image Icon { get; }

    public void AnimateHover();
    public void AnimatePressed();
    public void AnimateBaseState();
    public void AnimateShow();
    public void AnimateHide();
}

public class IconButton : MonoBehaviour, IIconButton
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color pressedColor;
    [SerializeField][Range(0.0f, 1.0f)] private float pressScaleFactor = 0.95f;

    public Image Icon => image;

    public event UnityAction onClick
    {
        add => button.onClick.AddListener(value);
        remove => button.onClick.RemoveListener(value);
    }

    public void AnimateShow()
    {
        AnimService.Instance.PlayAnim<ShowButton>(gameObject);
    }

    public void AnimateHide()
    {
        AnimService.Instance.PlayAnim<HideButton>(gameObject);
    }

    public void AnimateBaseState()
    {
        AnimService.Instance.PlayAnim<SetBaseButton>(gameObject);
    }

    public void AnimateHover()
    {
        AnimService.Instance.PlayAnim<HoverButton>(gameObject);
    }

    public void AnimatePressed()
    {
        AnimService.Instance.PlayAnim<PressButton>(gameObject);
    }

    private void Awake()
    {
        AnimService.Instance.BuildAnim<HoverButton>(gameObject)
            .Build();

        AnimService.Instance.BuildAnim<PressButton>(gameObject) 
            .Build();

        AnimService.Instance.BuildAnim<SetBaseButton>(gameObject)
            .Build();

        AnimService.Instance.BuildAnim<ShowButton>(gameObject)
            .Build();

        AnimService.Instance.BuildAnim<HideButton>(gameObject)
            .Build();
    }
}
