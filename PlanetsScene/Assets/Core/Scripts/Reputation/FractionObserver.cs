using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;


namespace Reptutation_Screen
{
    public interface IFractionObserver
    {
        void Enable();
        void Disable();
    }

    public class FractionObserver : IFractionObserver
    {
        [Inject] private IReptutationManager reptutationManager;
        [Inject] private IFractionSpawner fractionSpawner;
        
        private IFraction view;

        public FractionObserver(IFraction view)
        {
            this.view = view;
        }

        public void Enable()
        {
            reptutationManager.onReptutationChange += ChangeReputation;
            fractionSpawner.onUpdateInfo += UpdateInfo;
            view.SetVisibility(true);
        }

        public void Disable()
        {
            reptutationManager.onReptutationChange -= ChangeReputation;
            fractionSpawner.onUpdateInfo -= UpdateInfo;
        }

        private void UpdateInfo(FractionInfo info, IFraction fraction)
        {
            if (view != fraction)
                return;

            view.Name = info.Name;
            view.SetReputation(info.startReputation);
            view.Image = info.icon;
        }

        private void ChangeReputation(int reputation, IFraction fraction)
        {
            if (view != fraction)
                return;

            view.SetReputation(reputation);
        }

    }
}


