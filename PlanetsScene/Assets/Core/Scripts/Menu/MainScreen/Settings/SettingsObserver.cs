using System.Collections;
using System.Collections.Generic;
using Game_camera;
using Save_screen;
using Settings_Screen;
using UnityEngine;
using VContainer;

namespace Menu
{
    public interface ISettingsObserver
    {
        void Enable();
        void Disable();
    }

    public class SettingsObserver : ISettingsObserver
    {
        [Inject] private IMenuScreen menuScreen;

        [Inject] private ISaveScreen saveScreen;
        [Inject] private ISettingsScreen settingsScreen;
        
        [Inject] private ICameraView mainCamera;
        [Inject] private IPointerManager pointer;

        private IButtonView view;
        private Vector3 startPos;
        public SettingsObserver(IButtonView view)
        {
            this.view = view;
        }

        public void Enable()
        {
            startPos = view.Position;

            Vector3 animStartPos = startPos;
            animStartPos.x += menuScreen.XStartAnimOffset;
            view.Position = animStartPos;

            SetInterective();

            mainCamera.onEndMove += AnimatePos;

            saveScreen.onEnable += SetNotInterective;
            saveScreen.onDisable += SetInterective;

            settingsScreen.onEnable += SetNotInterective;
            settingsScreen.onDisable += SetInterective;
        }

        public void Disable()
        {
            SetNotInterective();

            mainCamera.onEndMove -= AnimatePos;

            saveScreen.onEnable -= SetNotInterective;
            saveScreen.onDisable -= SetInterective;

            settingsScreen.onEnable -= SetNotInterective;
            settingsScreen.onDisable -= SetInterective;
        }

        private void SetInterective()
        {
            pointer.OnPointerMove += ChangeStateByMouse;
            view.onClick += settingsScreen.Enable;
        }

        private void SetNotInterective()
        {
            pointer.OnPointerMove -= ChangeStateByMouse;
            view.onClick -= settingsScreen.Enable;

            view.LerpObjectState(0.0f);
        }

        private void AnimatePos()
        {
            view.AnimatePosition(startPos);
        }

        private void ChangeStateByMouse()
        {
            float outerPlanetRadius = GetOuterDist();
            float dist = GetMouseToObjDist();
            float alpha = 1.0f - (dist - outerPlanetRadius * view.OuterDistMultilpier) / (outerPlanetRadius * 0.5f);

            view.LerpObjectState(alpha);
        }

        private float GetMouseToObjDist()
        {
            Vector3 objPos = (Vector2)(view.ScreenPosition);
            Vector3 cursorPos = pointer.NowScreenPosition;

            //данная формула задает расстояние по квадрату
            return Vector3.Distance(objPos, cursorPos);
        }

        private float GetOuterDist()
        {
            return view.Size.magnitude;
        }
    }
}

