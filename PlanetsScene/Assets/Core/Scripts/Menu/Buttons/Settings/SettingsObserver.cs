using System.Collections;
using System.Collections.Generic;
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
        [Inject] private IPointerManager pointer;

        private IButtonView view;
        public SettingsObserver(IButtonView view)
        {
            this.view = view;
        }

        public void Enable()
        {
            pointer.OnPointerMove += ChangeStateByMouse;
        }

        public void Disable()
        {
            pointer.OnPointerMove -= ChangeStateByMouse;
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

            //������ ������� ������ ���������� �� ��������
            return Vector3.Distance(objPos, cursorPos);
        }

        private float GetOuterDist()
        {
            return view.Size.magnitude;
        }
    }
}

