using System.Collections;
using System.Collections.Generic;
using Game_camera;
using Save_screen;
using Settings_Screen;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace Menu
{
    public interface INewGameButtonObserver
    {
        void Enable();
        void Disable();
    }
    public class NewGameButtonObserver : INewGameButtonObserver
    {
        [Inject] private IMenuScreen menuScreen;
        [Inject] private ISettingsScreen settingsScreen;

        [Inject] private ICameraView mainCamera;
        [Inject] private IPointerManager pointer;
        [Inject] private ISaveScreen saveScreen;
        [Inject] private IPopUpsManager popUpsManager;

        private IButtonView view;
        private Vector3 startPos;

        private bool isInterective;
        public NewGameButtonObserver(IButtonView newGameButtonView)
        {
            this.view = newGameButtonView;
            isInterective = false;
        }
        public void Enable()
        {
            startPos = view.Position;

            Vector3 animStartPos = startPos;
            animStartPos.x += menuScreen.XStartAnimOffset;
            view.Position = animStartPos;

            mainCamera.onEndMove += AnimatePos;

            saveScreen.onEnable += SetNotInterective;
            saveScreen.onDisable += SetInterective;

            settingsScreen.onEnable += SetNotInterective;
            settingsScreen.onDisable += SetInterective;

            SetInterective();
        }

        public void Disable()
        {
            mainCamera.onEndMove -= AnimatePos;

            saveScreen.onEnable -= SetNotInterective;
            saveScreen.onDisable -= SetInterective;

            settingsScreen.onEnable -= SetNotInterective;
            settingsScreen.onDisable -= SetInterective;

            SetNotInterective();
        }

        private void SetInterective()
        {
            if (isInterective) return;
            
            isInterective = true;
            
            pointer.OnPointerMove += ChangeStateByMouse;
            view.onClick += CreateNewSave;
        }

        private void SetNotInterective()
        {
            isInterective = false;

            pointer.OnPointerMove -= ChangeStateByMouse;
            view.onClick -= CreateNewSave;

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

        private void CreateNewSave()
        {
            saveScreen.Enable();
            popUpsManager.SetNewSaveVisibility(true, null);
        }
    }
}
