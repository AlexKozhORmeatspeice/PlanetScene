using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using Planet_Window;
using Frontier_UI;

namespace Space_Screen
{
    public interface IPlanetObserver
    {
        void Enable();
        void Disable();
    }

    public class PlanetObserver : IPlanetObserver
    {
        [Inject] private IIconButtonMouseEvents btnMouse;
        [Inject] private IPlanetMouseEvents planetMouse;
        [Inject] private IPlanetWindow planetWindow;
        [Inject] private ITravelManager travelManager;

        private IPlanet view;
        private bool isChoosed;

        public PlanetObserver(IPlanet planet)
        {
            this.view = planet;
        }

        public void Enable()
        {
            SetInterective();
            SetBtnNotInterective();

            view.AnimateBase();

            planetWindow.onEnable += SetNotInterective;
            planetWindow.onDisable += SetInterective;

            travelManager.onTravelToPlanet += CheckChoosedPlanet;
        }

        public void Disable()
        {
            SetNotInterective();
            SetBtnNotInterective();

            planetWindow.onEnable -= SetNotInterective;
            planetWindow.onDisable -= SetInterective;

            travelManager.onTravelToPlanet -= CheckChoosedPlanet;
        }

        private void CheckChoosedPlanet(IPlanet choosedPlanet)
        {
            if (view == choosedPlanet && !isChoosed)
            {
                EnableAsChoosedPlanet();
            }
            else if (view != choosedPlanet && isChoosed)
            {
                DisableAsChoosedPlanet();
            }
        }

        private void EnableAsChoosedPlanet()
        {
            SetNotInterective();
            SetBtnInterective();

            planetWindow.onEnable -= SetNotInterective;
            planetWindow.onDisable -= SetInterective;

            view.AnimateChoose();
            isChoosed = true;
        }

        private void DisableAsChoosedPlanet()
        {
            SetInterective();
            SetBtnNotInterective();

            planetWindow.onEnable += SetNotInterective;
            planetWindow.onDisable += SetInterective;

            view.AnimateBase();
            isChoosed = false;
        }

        public void SetInterective()
        {
            planetMouse.OnMouseEnter += AnimateHover;
            planetMouse.OnMouseLeave += AnimateBase;
        }

        public void SetNotInterective()
        {
            planetMouse.OnMouseEnter -= AnimateHover;
            planetMouse.OnMouseLeave -= AnimateBase;
        }

        private void SetBtnInterective()
        {
            view.encBtn.AnimateBaseState();

            btnMouse.OnMouseEnter += AnimateBtnHover;
            btnMouse.OnMouseLeave += AnimateBtnBase;
            btnMouse.OnMouseClick += AnimateBtnPress;
        }

        private void SetBtnNotInterective()
        {
            btnMouse.OnMouseEnter -= AnimateBtnHover;
            btnMouse.OnMouseLeave -= AnimateBtnBase;
            btnMouse.OnMouseClick -= AnimateBtnPress;

            view.encBtn.AnimateBaseState();
        }

        private void AnimateHover(IPlanet planet)
        {
            if (planet != view) return;

            view.AnimateHover();
        }

        private void AnimateBase(IPlanet planet)
        {
            if (planet != view) return;

            view.AnimateBase();
        }

        private void AnimateBtnHover(IIconButton btn)
        {
            if (btn != view.encBtn) return;

            view.encBtn.AnimateHover();
        }

        private void AnimateBtnPress(IIconButton btn)
        {
            if (btn != view.encBtn) return;

            view.encBtn.AnimatePressed();
        }

        private void AnimateBtnBase(IIconButton btn)
        {
            if (btn != view.encBtn) return;

            view.encBtn.AnimateBaseState();
        }
    }

}