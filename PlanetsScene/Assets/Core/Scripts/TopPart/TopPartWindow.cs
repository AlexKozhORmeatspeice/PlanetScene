using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public interface ITopPartWindow
{

}

public class TopPartWindow : MonoBehaviour, ITopPartWindow, IStartable, IDisposable
{
    private ITopPart_SectorNameObserver sectorNameObserver;
    
    private ITopPart_GoBackObserver goBackPresenter;
    private ITopPart_ButtonsObserver buttonsObserver;

    [SerializeField] private TopPart_SectorName sectorNameView;
    
    [SerializeField] private TopPart_GoBack goBackBtnView;
    [SerializeField] private TopPart_ButtonsView buttonsView;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(goBackPresenter = new TopPart_GoBackObserver(goBackBtnView));
        resolver.Inject(sectorNameObserver = new TopPart_SectorNameObserver(sectorNameView));
        resolver.Inject(buttonsObserver = new TopPart_ButtonsObserver(buttonsView));
    }

    void IStartable.Start()
    {
        
    }

    public void Initialize()
    {
        goBackPresenter.Enable();
        sectorNameObserver.Enable();
        buttonsObserver.Enable();
    }

    public void Dispose()
    {
        goBackPresenter.Disable();
        sectorNameObserver.Disable();
        buttonsObserver.Disable();
    }

}
