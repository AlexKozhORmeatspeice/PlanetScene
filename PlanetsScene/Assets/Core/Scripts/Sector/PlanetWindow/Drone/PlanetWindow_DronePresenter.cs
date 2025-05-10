using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using VContainer;

public interface IPlanetWindow_DronePresenter
{
    event Action OnAnimDone;
    void Enable(List<Vector3> newTrajectory);
    void EndAnim();
}

public class PlanetWindow_DronePresenter : IPlanetWindow_DronePresenter
{
    private IPlanetWindow_Drone view;

    public event Action OnAnimDone;

    public PlanetWindow_DronePresenter(IPlanetWindow_Drone view)
    {
        this.view = view;
    }

    public void Enable(List<Vector3> newTrajectory)
    {
        view.Trajectory = newTrajectory;
        view.PlayAnim();
    }

    public void EndAnim()
    {
        OnAnimDone?.Invoke();
    }
}
