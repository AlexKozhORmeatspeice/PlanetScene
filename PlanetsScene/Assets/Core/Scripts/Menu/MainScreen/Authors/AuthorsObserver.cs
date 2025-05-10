using System.Collections;
using System.Collections.Generic;
using Game_camera;
using Save_screen;
using Settings_Screen;
using UnityEngine;
using VContainer;

namespace Menu
{
    public interface IAuthorsObserver
    {
        void Enable();
        void Disable();
    }

    public class AuthorsObserver : IAuthorsObserver
    {
        [Inject] private ISettingsScreen settingsScreen;
        [Inject] private ISaveScreen saveScreen;

        [Inject] private IMenuScreen menuScreen;
        [Inject] private ICameraView mainCamera;
        [Inject] private IPointerManager pointer;

        private IButtonView view;
        private Vector3 startPos;

        public AuthorsObserver(IButtonView view)
        {
            this.view = view;
        }

        public void Enable()
        {
            view.LerpObjectState(0.0f);
            
            startPos = view.Position;

            Vector3 animStartPos = startPos;
            animStartPos.x += menuScreen.XStartAnimOffset;
            view.Position = animStartPos;

            mainCamera.onEndMove += AnimatePos;
        }

        public void Disable()
        {
            SetNotIntrective();
            mainCamera.onEndMove -= AnimatePos;
        }

        private void SetIntrective()
        {
            pointer.OnPointerMove += ChangeStateByMouse;
        }

        private void SetNotIntrective()
        {
            pointer.OnPointerMove -= ChangeStateByMouse;
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

