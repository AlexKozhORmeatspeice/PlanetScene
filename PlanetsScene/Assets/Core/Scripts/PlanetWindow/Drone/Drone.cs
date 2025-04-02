using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public interface IDrone
{
    event Action onLand;
    void Land(Vector3 pos);
}
public class Drone : MonoBehaviour, IDrone
{
    private IPlanetWindow_DronePresenter presenter;

    [SerializeField] private PlanetWindow_Drone view;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform middlePoint;

    public event Action onStartLand;
    public event Action onLand;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(presenter = new PlanetWindow_DronePresenter(view));
        view.Init(presenter);
    }

    public void Land(Vector3 pos)
    {
        onStartLand?.Invoke();
        presenter.OnAnimDone += Land;

        presenter.Enable(GetTrajectory(pos));
    }

    private List<Vector3> GetTrajectory(Vector3 endPos)
    {
        List<Vector3> trajectory = new List<Vector3>();

        trajectory.Add(startPoint.position);
        trajectory.Add(middlePoint.position);
        trajectory.Add(endPos);

        return trajectory;
    }

    private void Land()
    {
        onLand?.Invoke();
    }
}
