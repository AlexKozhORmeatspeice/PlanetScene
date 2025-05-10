using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Menu
{

    public class MenuScreen : MonoBehaviour, IMenuScreen, IStartable, IDisposable
    {
        [Inject] private IObjectResolver resolver;

        private INewGameButtonObserver newGameObserver;
        private ILoadGameObserver loadGameObserver;
        private IContinueGameObserver continueGameObserver;
        private ISettingsObserver settingsObserver;
        private IAuthorsObserver authorsObserver;
        private IExitObserver exitObserver;
        private ILogoObserver logoObserver;
        private IMenuSideBarObserver menuSideBarObserver;

        [Header("Settings")]
        [SerializeField] private float elemsStartXOffset = 520.0f;

        [Header("Objs")]
        [SerializeField] private ButtonView newGameButtonView;
        [SerializeField] private ButtonView loadGameView;
        [SerializeField] private ButtonView continueGameView;
        [SerializeField] private ButtonView settingsView;
        [SerializeField] private ButtonView authorsView;
        [SerializeField] private ButtonView exitView;
        [SerializeField] private LogoView logo;
        [SerializeField] private MenuSideBarView menuSideBar;

        public float XStartAnimOffset => elemsStartXOffset;

        public void Initialize()
        {
            resolver.Inject(newGameObserver = new NewGameButtonObserver(newGameButtonView));
            resolver.Inject(loadGameObserver = new LoadGameObserver(loadGameView));
            resolver.Inject(continueGameObserver = new ContinueGameObserver(continueGameView));
            resolver.Inject(settingsObserver = new SettingsObserver(settingsView));
            resolver.Inject(authorsObserver = new AuthorsObserver(authorsView));
            resolver.Inject(exitObserver = new ExitObserver(exitView));
            resolver.Inject(logoObserver = new LogoObserver(logo));
            resolver.Inject(menuSideBarObserver = new MenuSideBarObserver(menuSideBar));


            newGameObserver.Enable();
            loadGameObserver.Enable();
            continueGameObserver.Enable();
            settingsObserver.Enable();
            authorsObserver.Enable();
            exitObserver.Enable();
            logoObserver.Enable();
            menuSideBarObserver.Enable();
        }

        public void Dispose()
        {
            newGameObserver.Disable();
            loadGameObserver.Disable();
            continueGameObserver.Disable();
            settingsObserver.Disable();
            authorsObserver.Disable();
            exitObserver.Disable();
            logoObserver.Disable();
            menuSideBarObserver.Disable();
        }

        public void Start() { }
    }

    public interface IMenuScreen
    {
        float XStartAnimOffset { get; }
    }
}