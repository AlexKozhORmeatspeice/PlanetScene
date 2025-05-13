using System.Collections;
using System.Collections.Generic;
using Frontier_UI;
using UnityEngine;

namespace Space_Screen
{
    public interface IFadePanelObserver
    {
        void Enable();
        void Disable();
    }

    public class FadePanelObserver : IFadePanelObserver
    {
        private IStartFadePanel fadePanel;

        public FadePanelObserver(IStartFadePanel fadePanel)
        {
            this.fadePanel = fadePanel;
        }

        public void Enable()
        {
            fadePanel.AnimateShowScreen();
        }

        public void Disable()
        {
            fadePanel.AnimateHideScreen();
        }
    }
}

