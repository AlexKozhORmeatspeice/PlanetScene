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
    private bool isBase;
    private bool isHover;

    public event UnityAction onClick
    {
        add => button.onClick.AddListener(value);
        remove => button.onClick.RemoveListener(value);
    }

    public void AnimateShow()
    {
        AnimService.Instance.PlayAnim<ShowIconButton>(gameObject);
    }

    public void AnimateHide()
    {
        AnimService.Instance.PlayAnim<HideIconButton>(gameObject);
    }

    public void AnimateBaseState()
    {
        if (isBase) return;

        isBase = true;
        isHover = false;

        AnimService.Instance.PlayAnim<SetBaseIconButton>(gameObject);
    }

    public void AnimateHover()
    {
        if (isHover) return;

        isBase = false;
        isHover = true;

        AnimService.Instance.PlayAnim<HoverIconButton>(gameObject);
    }

    public void AnimatePressed()
    {
        isBase = false;
        isHover = false;

        AnimService.Instance.PlayAnim<PressIconButton>(gameObject);
    }

    private void Awake()
    {
        isBase = true;
        isHover = false;


        AnimService.Instance.BuildAnim<HoverIconButton>(gameObject)
            .Build();

        AnimService.Instance.BuildAnim<PressIconButton>(gameObject)
            .SetCallback(AnimateBaseState)
            .Build();

        AnimService.Instance.BuildAnim<SetBaseIconButton>(gameObject)
            .Build();

        AnimService.Instance.BuildAnim<ShowIconButton>(gameObject)
            .Build();

        AnimService.Instance.BuildAnim<HideIconButton>(gameObject)
            .Build();
    }
}
