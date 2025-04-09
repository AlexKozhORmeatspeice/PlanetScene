using UnityEngine;
using VContainer;

namespace Space_Screen
{
    public interface IPlanetToolTipObserver
    {
        void Enable();
        void Disable();
    }

    public class PlanetToolTipObserver : IPlanetToolTipObserver
    {
        private IPlanetTooltipSystem tooltipSystem;
        [Inject] private IPointerManager pointer;

        private IToolTip view;

        private bool isMirroredX;
        private bool isMirroredY;

        public PlanetToolTipObserver(IToolTip view, IPlanetTooltipSystem tooltipSystem)
        {
            this.view = view;
            this.tooltipSystem = tooltipSystem;
        }

        public void Enable()
        {
            isMirroredX = false;
            isMirroredY = false;

            view.Hide();

            tooltipSystem.OnPointerEnterPlanet += ShowToolTip;
            tooltipSystem.OnPointerLeavePlanet += HideToolTip;
            
            tooltipSystem.onTooltipOutScreen += CheckOutScreenVisibility;
            tooltipSystem.onTooltipInScreen += CheckInScreenVisibility;

            pointer.OnUpdate += SetPos;
        }


        public void Disable()
        {
            tooltipSystem.OnPointerEnterPlanet -= ShowToolTip;
            tooltipSystem.OnPointerLeavePlanet -= HideToolTip;

            tooltipSystem.onTooltipOutScreen -= CheckOutScreenVisibility;
            tooltipSystem.onTooltipInScreen -= CheckInScreenVisibility;

            pointer.OnUpdate -= SetPos;
        }

        private void ShowToolTip(IPlanet planet)
        {
            if (planet == null)
                return;

            view.Name = planet.Name;
            view.Description = planet.Description;
            
            view.Show();
        }

        private void HideToolTip(IPlanet planet)
        {
            view.Hide();
        }


        //ќтражаем по оси если вышел за границу
        //≈сли вернулс€ - соотвественно возвращаем обратно
        //ѕровер€ем только одну из сторон дл€ каждой из осей
        private void CheckOutScreenVisibility(ScreenSide side)
        {
            if (side == ScreenSide.right && !isMirroredX)
            {
                MirrorX();
                isMirroredX = true;
            }

            if (side == ScreenSide.top && !isMirroredY)
            {
                MirrorY();
                isMirroredY = true;
            }
        }

        private void CheckInScreenVisibility(ScreenSide side)
        {
            if (side == ScreenSide.right && isMirroredX)
            {
                MirrorX();
                isMirroredX = false;
            }

            if (side == ScreenSide.top && isMirroredY)
            {
                MirrorY();
                isMirroredY = false;
            }
        }

        private void MirrorX()
        {
            float distX = view.IconPos.x - view.Position.x;
            float newX = view.Position.x + (-distX);

            view.IconPos = new Vector3(newX, view.IconPos.y, view.IconPos.z);
        }

        private void MirrorY()
        {
            float distY = view.IconPos.y - view.Position.y;
            float newY = view.Position.y + (-distY);

            view.IconPos = new Vector3(view.IconPos.x, newY, view.IconPos.z);
        }

        private void SetPos()
        {
            Vector3 mousePos = pointer.NowWorldPosition;
            view.Position = new Vector3(mousePos.x, mousePos.y, view.Position.z);
        }
    }
}
