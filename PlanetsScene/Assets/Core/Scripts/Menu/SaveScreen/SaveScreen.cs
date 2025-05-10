using System;
using System.Collections;
using System.Collections.Generic;
using Game_UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Save_screen
{
    public interface ISaveScreen
    {
        event Action onEnable;
        event Action onDisable;
        void Enable();
        void Disable();
    }

    public class SaveScreen : MonoBehaviour, ISaveScreen, IStartable
    {
        [Inject] private IObjectResolver resolver;

        [Header("Objs")]
        [SerializeField] private TextInputPopUp newSavePopUp;
        [SerializeField] private IconButton closeSavesButton;
        [SerializeField] private TextPopUp rewritePopUp;
        [SerializeField] private TextPopUp deletePopUp;
        [SerializeField] private TextPopUp loadPopUp;
        [SerializeField] private ScrollBar scrollBar;
        [SerializeField] private SaveScreenView saveScreenView;

        private INewSavePopUpObserver newSavePopUpObserver;
        private IRewritePopUpObserver rewritePopUpObserver;
        private IDeletePopUpObserver deletePopUpObserver;
        private ILoadPopUpObserver loadPopUpObserver;
        private ICLoseScreen closeScreenObserver;
        private ISaveScreenObserver saveScreenObserver;

        public event Action onEnable;
        public event Action onDisable;

        public void Initialize()
        {
            resolver.Inject(newSavePopUpObserver = new NewSavePopUpObserver(newSavePopUp));
            resolver.Inject(rewritePopUpObserver = new RewritePopUpObserver(rewritePopUp));
            resolver.Inject(deletePopUpObserver = new DeletePopUpObserver(deletePopUp));
            resolver.Inject(loadPopUpObserver = new LoadPopUpObserver(loadPopUp));
            resolver.Inject(closeScreenObserver = new CloseSaveScreenButtonObserver(closeSavesButton, this));

            resolver.Inject(saveScreenObserver = new SaveScreenObserver(saveScreenView));
        }

        public void Enable()
        {
            onEnable?.Invoke();

            newSavePopUpObserver.Enable();
            rewritePopUpObserver.Enable();
            deletePopUpObserver.Enable();
            loadPopUpObserver.Enable();
            closeScreenObserver.Enable();
            saveScreenObserver.Enable();
        }

        public void Disable()
        {
            onDisable?.Invoke();

            newSavePopUpObserver.Disable();
            rewritePopUpObserver.Disable();
            deletePopUpObserver.Disable();
            loadPopUpObserver.Disable();
            closeScreenObserver.Disable();
            saveScreenObserver.Disable();
        }

        public void Start() { }
    }
}
