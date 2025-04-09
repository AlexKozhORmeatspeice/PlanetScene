using System.Collections;
using System.Collections.Generic;
using Space_Screen;
using UnityEngine;
using VContainer;

namespace Planet_Window
{
    public interface IPOIToolTipObserver
    {
        void Enable();
        void Disable();
    }
    public class POIToolTipObserver : IPOIToolTipObserver
    {
        private IPOITooltipSystem tooltipSystem;
        [Inject] private IPointerManager pointer;

        private IToolTip view;

        private bool isMirroredX;
        private bool isMirroredY;

        public POIToolTipObserver(IToolTip view, IPOITooltipSystem tooltipSystem)
        {
            this.view = view;
            this.tooltipSystem = tooltipSystem;
        }

        public void Enable()
        {
            isMirroredX = false;
            isMirroredY = false;

            view.Hide();

            tooltipSystem.OnPointerEnterPOI += ShowToolTip;
            tooltipSystem.OnPointerLeavePOI += HideToolTip;

            tooltipSystem.onTooltipOutScreen += CheckOutScreenVisibility;
            tooltipSystem.onTooltipInScreen += CheckInScreenVisibility;

            pointer.OnUpdate += SetPos;
        }


        public void Disable()
        {
            tooltipSystem.OnPointerEnterPOI -= ShowToolTip;
            tooltipSystem.OnPointerLeavePOI -= HideToolTip;

            tooltipSystem.onTooltipOutScreen -= CheckOutScreenVisibility;
            tooltipSystem.onTooltipInScreen -= CheckInScreenVisibility;

            pointer.OnUpdate -= SetPos;
        }

        private void ShowToolTip(IPointOfInterest poi)
        {
            if (poi == null)
                return;

            view.Name = poi.GetInfo.nameOfPoint;
            view.Lable = poi.GetInfo.type.ToString();
            view.Description = poi.GetInfo.desc;

            view.Show();
        }

        private void HideToolTip(IPointOfInterest poi)
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