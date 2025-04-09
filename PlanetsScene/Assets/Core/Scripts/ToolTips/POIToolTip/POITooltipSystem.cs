using System;
using Space_Screen;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace Planet_Window
{
    public interface IPOITooltipSystem
    {
        event Action<IPointOfInterest> OnPointerEnterPOI;
        event Action<IPointOfInterest> OnPointerLeavePOI;
        event Action<ScreenSide> onTooltipOutScreen;
        event Action<ScreenSide> onTooltipInScreen;
    }

    public class POITooltipSystem : MonoBehaviour, IPOITooltipSystem, IStartable, IDisposable
    {
        [SerializeField] private ToolTip toolTip;

        [Inject] private IPlanetWindow planetWindow;
        [Inject] private IScaner scaner;
        [Inject] private IPOIMouseEvents poiMouse;
        private bool canEnable;

        protected IPOIToolTipObserver toolTipObserver;

        public event Action<IPointOfInterest> OnPointerEnterPOI;
        public event Action<IPointOfInterest> OnPointerLeavePOI;
        public event Action<ScreenSide> onTooltipOutScreen;
        public event Action<ScreenSide> onTooltipInScreen;

        private Dictionary<ScreenSide, bool> inScreenBySide;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(toolTipObserver = new POIToolTipObserver(toolTip, this));
        }

        public void Initialize()
        {
            inScreenBySide = new Dictionary<ScreenSide, bool>
            {
                { ScreenSide.left, true },
                { ScreenSide.right, true },
                { ScreenSide.top, true },
                { ScreenSide.bottom, true }
            };

            canEnable = true;
            toolTipObserver.Enable();

            poiMouse.OnMouseEnter += SetPOIOnOnter;
            poiMouse.OnMouseLeave += DisablePOIOnLeave;

            planetWindow.onChangeStatus += SetTooltipCanEnable;
            scaner.onChangeIsScanning += SetTooltipCantEnable;
        }

        public void Dispose()
        {
            toolTipObserver.Disable();

            poiMouse.OnMouseEnter -= SetPOIOnOnter;
            poiMouse.OnMouseLeave -= DisablePOIOnLeave;

            planetWindow.onChangeStatus -= SetTooltipCanEnable;
            scaner.onChangeIsScanning -= SetTooltipCantEnable;
        }

        public void Start() { }

        private void SetPOIOnOnter(IPointOfInterest poi)
        {
            if (!canEnable || (!poi.CanScanerSee && !poi.IsVisible) || !poi.IsVisible)
                return;
         
            OnPointerEnterPOI?.Invoke(poi);
        }

        private void DisablePOIOnLeave(IPointOfInterest poi)
        {
            OnPointerLeavePOI?.Invoke(poi);
        }

        private void SetTooltipCanEnable(bool canEnable)
        {
            this.canEnable = canEnable;
        }
        private void SetTooltipCantEnable(bool cantEnable)
        {
            canEnable = !cantEnable;
        }

        public void FixedUpdate()
        {
            CheckScreenSides();
        }

        private void CheckScreenSides()
        {
            Vector3 sreenPos = Camera.main.WorldToScreenPoint(toolTip.Position + toolTip.StartOffset);

            if (sreenPos.x + toolTip.ScreenSize.x > Screen.width)
            {
                SignalOutOfScreen(ScreenSide.right);
            }
            else
            {
                SignalInScreen(ScreenSide.right);
            }

            if (sreenPos.y + toolTip.ScreenSize.y > Screen.height)
            {
                SignalOutOfScreen(ScreenSide.top);
            }
            else
            {
                SignalInScreen(ScreenSide.top);
            }
        }

        private void SignalOutOfScreen(ScreenSide side)
        {
            if (!inScreenBySide[side])
                return;

            onTooltipOutScreen?.Invoke(side);
            inScreenBySide[side] = false;
        }

        private void SignalInScreen(ScreenSide side)
        {
            if (inScreenBySide[side])
                return;

            onTooltipInScreen?.Invoke(side);
            inScreenBySide[side] = true;
        }
    }
}
