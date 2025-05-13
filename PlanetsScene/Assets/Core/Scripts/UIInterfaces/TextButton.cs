using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Frontier_anim;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface ITextButton
{
    event UnityAction onClick;

    CanvasGroup CanvasGroup { get; }

    public void AnimateHover();
    public void AnimatePressed();
    public void AnimateBaseState();
    public void AnimateHide();
    public void AnimateShow();
}
public class TextButton : MonoBehaviour, ITextButton
{
    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup => canvasGroup;

    public event UnityAction onClick
    {
        add => button.onClick.AddListener(value);
        remove => button.onClick.RemoveListener(value);
    }

    bool isPressed;
    bool isBase;
    bool isHover;
    bool isHidden;
    public void AnimateShow()
    {
        isHidden = false;
        AnimService.Instance.PlayAnim<ShowTxtBtn>(gameObject);
    }

    public void AnimateHide()
    {
        isHidden = true;
        AnimService.Instance.PlayAnim<HideTxtBtn>(gameObject);
    }

    public void AnimateBaseState()
    {
        if (isBase || isHidden) return;

        isBase = true;
        isHover = false;
        isPressed = false;

        AnimService.Instance.PlayAnim<SetBaseTxtBtn>(gameObject);
    }

    public void AnimateHover()
    {
        if (isHover || isHidden) return;

        isBase = false;
        isHover = true;
        isPressed = false;

        AnimService.Instance.PlayAnim<HoverTxtBtn>(gameObject);
    }

    public void AnimatePressed()
    {
        if (isPressed || isHidden) return;

        isBase = false;
        isHover = false;
        isPressed = true;

        AnimService.Instance.PlayAnim<PressTxtBtn>(gameObject);
    }

   

    private void Awake()
    {
        isHidden = false;
        isBase = false;
        isHover = false;
        isPressed = false;    

        AnimService.Instance.BuildAnim<HoverTxtBtn>(gameObject)
            .Build();

        AnimService.Instance.BuildAnim<PressTxtBtn>(gameObject)
            .SetCallback(AnimateBaseState)
            .Build();

        AnimService.Instance.BuildAnim<SetBaseTxtBtn>(gameObject)
            .Build();

        AnimService.Instance.BuildAnim<HideTxtBtn>(gameObject)
            .SetCanvasGroup(canvasGroup)
            .Build();

        AnimService.Instance.BuildAnim<ShowTxtBtn>(gameObject)
            .SetCanvasGroup(canvasGroup)
            .Build();
    }
}
