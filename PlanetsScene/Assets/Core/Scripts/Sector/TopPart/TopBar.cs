using System;
using Top_Bar;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Space_TopBar
{
    public interface ITopBar
    {

    }

    public class TopBar : MonoBehaviour, IStartable, IDisposable
    {
        [SerializeField] private TopBarView view;
        [SerializeField] private IconButton backBtn;

        private ITopBarObserver observer;
        private IBackBtnObserver backBtnObserver;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(observer = new TopBarObserver(view));
            resolver.Inject(backBtnObserver = new BackBtnObserver(backBtn));
        }

        public void Initialize()
        {
            observer.Enable();
            backBtnObserver.Enable();
        }

        public void Dispose()
        {
            observer.Disable();
            backBtnObserver.Disable();
        }

        public void Start() { }
    }
}