using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public interface IPlanetWindow_ScanerObserver
{
    void Enable();
    void Disable();
}

public class PlanetWindow_ScanerObserver : IPlanetWindow_ScanerObserver
{
    [Inject] private IPointerManager pointer;
    private IPlanetWindow_Scaner view;
    private float lastSpeed;
    public PlanetWindow_ScanerObserver(IPlanetWindow_Scaner view)
    {
        this.view = view;
        lastSpeed = 0.0f;
    }

    public void Enable()
    {
        pointer.OnUpdate += ChangeSize;
    }
     
    public void Disable()
    {
        pointer.OnUpdate -= ChangeSize;
    }

    private void ChangeSize()
    {
        float speed = Mathf.Pow(pointer.Speed.magnitude, view.MouseSpeedInfluence);
        lastSpeed = Mathf.Lerp(lastSpeed, speed, Time.deltaTime * view.SpeedOfChange);
        lastSpeed = Mathf.Clamp01(lastSpeed);

        view.ChangeSize(lastSpeed);
    }
}
