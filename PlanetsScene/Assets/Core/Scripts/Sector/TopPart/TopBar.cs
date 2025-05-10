using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Top_Bar
{
    public interface ITopBar
    {
        void SetButtonsVisability(bool isVisible);
    }

    public class TopBar : MonoBehaviour, ITopBar, IStartable, IDisposable
    {
        private ISectorNameObserver sectorNameObserver;
        private IBackToPlanetButtonObserver goBackPresenter;
        private ITopPart_ButtonsObserver buttonsObserver;

        [SerializeField] private TopPart_SectorName sectorNameView;
        [SerializeField] private IconButton goBackBtnView;
        [SerializeField] private GameObject buttonsContent;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(goBackPresenter = new BackToPlanetButtonObserver(goBackBtnView));
            resolver.Inject(sectorNameObserver = new SectorNameObserver(sectorNameView));
            resolver.Inject(buttonsObserver = new TopPart_ButtonsObserver(this));
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

        public void SetButtonsVisability(bool isVisible)
        {
            buttonsContent.SetActive(isVisible);
        }

        void IStartable.Start() {}
    }
}
