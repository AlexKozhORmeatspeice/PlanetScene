using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

public interface IPlanetWindow_ScanerButton
{
    event UnityAction onClick;
    void SetStatus(bool status);
}

public class PlanetWindow_ScanerButton : MonoBehaviour, IPlanetWindow_ScanerButton, IDisposable
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonTxt;
    [SerializeField] private string activeScannerText;
    [SerializeField] private string notActiveScannerText;

    private bool isScanning;

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
}
