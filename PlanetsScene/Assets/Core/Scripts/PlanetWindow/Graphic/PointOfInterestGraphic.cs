using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;
public interface IGraphic
{
    void Enable();
    void Disable();
}

public interface IPointOfInterestGraphic : IGraphic
{

}

public class PointOfInterestGraphic : MonoBehaviour, IPointOfInterestGraphic, IDisposable
{
    [Inject] private ITravelManager travelManager;
    [Inject] private IScaner scaner;
    private IGraphicObserver observer;

    [SerializeField] private GraphicView view;
    [SerializeField] private float amplitudeSpeedOfChange = 2.0f;
    [SerializeField] private float minAmplitude;
    [SerializeField] private float maxAmplitude;

    private float nowAmplitude;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(observer = new GraphicObserver(view));
        nowAmplitude = minAmplitude;
    }

    private float Func(float x)
    {
        float countMaxs = scaner.SeePointValueByPOI.Keys.Count;
        float XMax = 10.0f;
        float areaSize = XMax / countMaxs;

        float res = 0.0f;
        float nowPos = areaSize / 2.0f;
        int i = 1;
        foreach(IPointOfInterest poi in scaner.SeePointValueByPOI.Keys)
        {
            XMax -= areaSize / 2.0f;
            float partAmplitude = Mathf.Clamp(scaner.SeePointValueByPOI[poi], 0.0f, float.MaxValue);
            
            res += NormalDistribution(x - nowPos, partAmplitude);
            nowPos += areaSize;
            i++;
        }

        return res;
    }

    private float NormalDistribution(float x, float amplitude)
    {
        return amplitude*Mathf.Exp((-1.0f)*x*x);
    }

    public void Dispose()
    {
        Disable();
    }

    public void Enable()
    {
        observer.Enable();
        observer.SetFunction(Func);
    }

    public void Disable()
    {
        observer.Disable();
    }
}
