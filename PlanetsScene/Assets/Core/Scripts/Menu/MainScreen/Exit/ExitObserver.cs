using System.Collections;
using System.Collections.Generic;
using Game_camera;
using Save_screen;
using Settings_Screen;
using UnityEngine;
using VContainer;


namespace Menu
{
    public interface IExitObserver
    {
        void Enable();
        void Disable();
    }
    public class ExitObserver : IExitObserver
    {
        [Inject] private ISaveScreen saveScreen;
        [Inject] private ISettingsScreen settingsScreen;

        [Inject] private IMenuScreen menuScreen;
        [Inject] private ICameraView mainCamera;
        [Inject] private IPointerManager pointer;

        private IButtonView view;
        private Vector3 startPos;

        private bool isInterective;
        public ExitObserver(IButtonView view)
        {
            this.view = view;
            isInterective = false;
        }

        public void Enable()
        {
            startPos = view.Position;

            Vector3 animStartPos = startPos;
            animStartPos.x += menuScreen.XStartAnimOffset;
            view.Position = animStartPos;

            mainCamera.onEndMove += AnimatePos;

            SetInterective();

            saveScreen.onEnable += SetNotInterective;
            saveScreen.onDisable += SetInterective;

            settingsScreen.onEnable += SetNotInterective;
            settingsScreen.onDisable += SetInterective;
        }

        public void Disable()
        {
            SetNotInterective();

            mainCamera.onEndMove -= AnimatePos;

            pointer.OnPointerMove -= ChangeStateByMouse;
            view.onClick -= CloseGame;

            saveScreen.onEnable -= SetNotInterective;
            saveScreen.onDisable -= SetInterective;

            settingsScreen.onEnable -= SetNotInterective;
            settingsScreen.onDisable -= SetInterective;
        }

        private void SetInterective()
        {
            if (isInterective) return;
            
            isInterective = true;

            pointer.OnPointerMove += ChangeStateByMouse;
            view.onClick += view.AnimatePress;
            view.onClick += CloseGame;
        }

        private void SetNotInterective()
        {
            isInterective = false;

            pointer.OnPointerMove -= ChangeStateByMouse;
            view.onClick -= CloseGame;
            view.onClick -= view.AnimatePress;
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

        private void CloseGame()
        {
            Debug.Log("Closed game");
            Application.Quit();
        }

        private float GetOuterDist()
        {
            return view.Size.magnitude;
        }
    }

}
