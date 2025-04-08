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

    /*
     * В качестве базовой функции использовал вариацию нормального распределения по Гауссу.
     * Каждое распределеине принадлежит одной из точек интереса.
     * 1. Смещаю графики по X, чтобы они делили равные по длине участки
     * 2. Суммирую графики
     * 3. Получаю итоговый график
     * 
     * Проблема: данный метод может есть достаточно много ресурсов, потому что функция
     * тяжеловесная для просчетов. И ее приходится просчитывать каждый кадр.
     * Для 2-3 точек интереса это не критично, но если их будет 10-12, могут начаться
     * просады FPS. 
     * 
     * TODO: Если станет критичным возможно нужно будет написать шейдер собственный,
     * но пока оставил так
     */
    private float Func(float x)
    {
        float countMaxs = scaner.SeePointValueByPOI.Keys.Count;
        float XMax = 10.0f;
        float areaSize = XMax / countMaxs;
        float halfArea = areaSize / 2.0f;

        float res = 0.0f;
        float nowPos = halfArea;
        int i = 1;
        foreach(IPointOfInterest poi in scaner.SeePointValueByPOI.Keys)
        {
            XMax -= halfArea;
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
        observer.SetFunction(Func);
        observer.Enable();
    }

    public void Disable()
    {
        observer.Disable();
    }
}
