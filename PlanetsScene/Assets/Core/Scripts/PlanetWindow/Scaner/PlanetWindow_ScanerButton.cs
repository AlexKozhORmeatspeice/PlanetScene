using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public interface IPlanetWindow_ScanerButton
{
    void Init(IPlanetWindow_ScanerButtonPresenter presenter);
    void OnButtonClick();
    void SetStatus(bool status);
}

public class PlanetWindow_ScanerButton : MonoBehaviour, IPlanetWindow_ScanerButton, IDisposable
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonTxt;
    [SerializeField] private string activeScannerText;
    [SerializeField] private string notActiveScannerText;

    private bool isScanning;

    private IPlanetWindow_ScanerButtonPresenter presenter;

    public void Init(IPlanetWindow_ScanerButtonPresenter presenter)
    {
        button.onClick.RemoveAllListeners();

        this.presenter = presenter;
        presenter.Init(this);

        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        bool isScanning = presenter.ChangeScanerStatus();

        SetStatus(isScanning);
    }

    public void Dispose()
    {
        button.onClick.RemoveAllListeners();
    }

    public void SetStatus(bool status)
    {
        if (status)
        {
            buttonTxt.text = activeScannerText;
        }
        else
        {
            buttonTxt.text = notActiveScannerText;
        }
    }
}
