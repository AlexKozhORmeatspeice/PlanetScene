using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game_UI;
using VContainer;
using System;

namespace Settings_Screen
{
    public interface ISettingsScreen
    {
        event Action onEnable;
        event Action onDisable;
        void Enable();
        void Disable();
    }

    public class SettingsScreen : MonoBehaviour, ISettingsScreen
    {
        [SerializeField] private SettingsSlotSlider brightnessSlider;
        [SerializeField] private SettingsSlotSlider soundSlider;
        [SerializeField] private SettingsSlotSlider musicSlider;
        [SerializeField] private SettingsSlotSlider mouseSensSlider;
        
        [SerializeField] private SettingsSlotDropdown languageDropdown;
        [SerializeField] private SettingsSlotDropdown resolutionDropdown;
        
        [SerializeField] private SettingsSlotToggle windowModeToggle;
        
        [SerializeField] private IconButton closeSettingsButton;
        [SerializeField] private SettingsView settingsView;

        private ILanguageObserver languageObserver;
        private ISoundObserver soundObserver;
        private IMusicObserver musicObserver;
        private IResolutionObserver resolutionObserver;
        private IWindowModeObserver windowModeObserver;
        private IBrightnessObserver brightnessObserver;
        private IMouseSensetivityObserver mouseSensetivityObserver;
        private ICloseSettingsButtonObserver closeSettingsButtonObserver;
        private ISettingsObserver settingsObserver;

        public event Action onEnable;
        public event Action onDisable;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(languageObserver = new LanguageObserver(languageDropdown));
            resolver.Inject(soundObserver = new SoundObserver(soundSlider));
            resolver.Inject(musicObserver = new MusicObserver(musicSlider));
            resolver.Inject(resolutionObserver = new ResolutionObserver(resolutionDropdown));
            resolver.Inject(windowModeObserver = new WindowModeObserver(windowModeToggle));
            resolver.Inject(brightnessObserver = new BrightnessObserver(brightnessSlider));
            resolver.Inject(mouseSensetivityObserver = new MouseSensetivityObserver(mouseSensSlider));
            resolver.Inject(closeSettingsButtonObserver = new CloseSettingsButtonObserver(closeSettingsButton, this));
            resolver.Inject(settingsObserver = new SettingsObserver(settingsView));
        }

        public void Enable()
        {
            languageObserver.Enable();
            soundObserver.Enable();
            musicObserver.Enable();
            resolutionObserver.Enable();
            windowModeObserver.Enable();
            brightnessObserver.Enable();
            mouseSensetivityObserver.Enable();
            closeSettingsButtonObserver.Enable();
            
            settingsObserver.Enable();
            
            onEnable?.Invoke();
        }

        public void Disable()
        {
            settingsObserver.Disable();
            
            languageObserver.Disable();
            soundObserver.Disable();
            musicObserver.Disable();
            resolutionObserver.Disable();
            windowModeObserver.Disable();
            brightnessObserver.Disable();
            mouseSensetivityObserver.Disable();
            closeSettingsButtonObserver.Disable();

            onDisable?.Invoke();
        }
    }
}