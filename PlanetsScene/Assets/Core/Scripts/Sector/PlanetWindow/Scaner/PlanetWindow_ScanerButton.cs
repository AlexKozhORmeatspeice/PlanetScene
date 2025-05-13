using System;
using Frontier_anim;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

public interface IPlanetWindow_ScanerButton : ITextButton
{
    void SetStatus(bool status);
}

public class PlanetWindow_ScanerButton : MonoBehaviour, IPlanetWindow_ScanerButton, IDisposable
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonTxt;
    [SerializeField] private string activeScannerText;
    [SerializeField] private string notActiveScannerText;

    private bool isScanning;

    private bool isBase;
    private bool isHover;
    private bool isPress;

    public CanvasGroup CanvasGroup => null;

    public event UnityAction onClick
    {
        add => button.onClick.AddListener(value);
        remove => button.onClick.RemoveListener(value);
    }

    public void Dispose()
    {
        button.onClick.RemoveAllListeners();
    }

    public void SetStatus(bool isActive)
    {
        if (isActive)
        {
            buttonTxt.text = activeScannerText;
        }
        else
        {
            buttonTxt.text = notActiveScannerText;
        }
    }

    public void AnimateBaseState()
    {
        if (isBase) return;

        isBase = true;
        isHover = false;
        isPress = false;

        AnimService.Instance.PlayAnim<SetBaseTxtBtn>(gameObject);
    }

    public void AnimateHover()
    {
        if (isHover) return;

        isBase = false;
        isHover = true;
        isPress = false;


        AnimService.Instance.PlayAnim<HoverTxtBtn>(gameObject);
    }

    public void AnimatePressed()
    {
        if (isPress) return;

        isBase = false;
        isHover = false;
        isPress = true;

        AnimService.Instance.PlayAnim<PressTxtBtn>(gameObject);
    }

    public void AnimateHide()
    {
        //
    }

    public void AnimateShow()
    {
        //
    }

    private void Awake()
    {
        AnimService.Instance.BuildAnim<HoverTxtBtn>(gameObject)
            .Build();

        AnimService.Instance.BuildAnim<PressTxtBtn>(gameObject)
            .SetCallback(AnimateBaseState)
            .Build();

        AnimService.Instance.BuildAnim<SetBaseTxtBtn>(gameObject)
            .Build();
    }
}
