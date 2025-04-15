using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Menu
{
    public class MenuScreen : MonoBehaviour, IMenuScreen, IStartable, IDisposable
    {
        private INewGameButtonObserver newGameObserver;
        private ILoadGameObserver loadGameObserver;
        private IContinueGameObserver continueGameObserver;
        private ISettingsObserver settingsObserver;
        private IAuthorsObserver authorsObserver;
        private IExitObserver exitObserver;

        [Header("Buttons")]
        [SerializeField] private ButtonView newGameButtonView;
        [SerializeField] private ButtonView loadGameView;
        [SerializeField] private ButtonView continueGameView;
        [SerializeField] private ButtonView settingsView;
        [SerializeField] private ButtonView authorsView;
        [SerializeField] private ButtonView exitView;

        [Inject]        
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(newGameObserver = new NewGameButtonObserver(newGameButtonView));
            resolver.Inject(loadGameObserver = new LoadGameObserver(loadGameView));
            resolver.Inject(continueGameObserver = new ContinueGameObserver(continueGameView));
            resolver.Inject(settingsObserver = new SettingsObserver(settingsView));
            resolver.Inject(authorsObserver = new AuthorsObserver(authorsView));
            resolver.Inject(exitObserver = new ExitObserver(exitView));
        }

        public void Initialize()
        {
            newGameObserver.Enable();
            loadGameObserver.Enable();
            continueGameObserver.Enable();
            settingsObserver.Enable();
            authorsObserver.Enable();
            exitObserver.Enable();
        }

        public void Dispose()
        {
            newGameObserver.Disable();
            loadGameObserver.Disable();
            continueGameObserver.Disable();
            settingsObserver.Disable();
            authorsObserver.Disable();
            exitObserver.Disable();
        }

        public void Start() { }
    }
    public interface IMenuScreen
    {

    }
}