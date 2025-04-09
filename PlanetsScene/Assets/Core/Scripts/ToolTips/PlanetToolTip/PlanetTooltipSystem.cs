using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;
using Planet_Window;
using System.Collections.Generic;

namespace Space_Screen
{
    public interface IPlanetTooltipSystem
    {
        event Action<IPlanet> OnPointerEnterPlanet;
        event Action<IPlanet> OnPointerLeavePlanet;
        event Action<ScreenSide> onTooltipOutScreen;
        event Action<ScreenSide> onTooltipInScreen;
    }

    public class PlanetTooltipSystem : MonoBehaviour, IPlanetTooltipSystem, IStartable, IDisposable
    {
        [SerializeField] private ToolTip toolTip;
        
        [Inject] private IPlanetWindow planetWindow; 
        [Inject] private ITravelManager travelManager;
        [Inject] private IPlanetMouseEvents planetMouse;
        private bool canEnable;

        protected IPlanetToolTipObserver toolTipObserver;

        public event Action<IPlanet> OnPointerEnterPlanet;
        public event Action<IPlanet> OnPointerLeavePlanet;
        public event Action<ScreenSide> onTooltipOutScreen;
        public event Action<ScreenSide> onTooltipInScreen;

        private Dictionary<ScreenSide, bool> inScreenBySide;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(toolTipObserver = new PlanetToolTipObserver(toolTip, this));
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

            planetMouse.OnMouseEnter += SetPlanet;
            planetMouse.OnMouseLeave += DisablePlanet;

            planetWindow.onChangeStatus += SetCantShowTooltip;
        }

        public void Dispose()
        {
            toolTipObserver.Disable();

            planetMouse.OnMouseEnter -= SetPlanet;
            planetMouse.OnMouseLeave -= DisablePlanet;
            
            planetWindow.onChangeStatus -= SetCantShowTooltip;
        }

        public void Start() { }

        private void SetPlanet(IPlanet planet)
        {
            if (travelManager.IsNowPlanet(planet) || !canEnable)
                return;

            OnPointerEnterPlanet?.Invoke(planet);
        }

        private void DisablePlanet(IPlanet planet)
        {
            if (travelManager.IsNowPlanet(planet))
                return;

            OnPointerLeavePlanet?.Invoke(planet);
        }

        private void SetCantShowTooltip(bool isCant)
        {
            canEnable = !isCant;
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