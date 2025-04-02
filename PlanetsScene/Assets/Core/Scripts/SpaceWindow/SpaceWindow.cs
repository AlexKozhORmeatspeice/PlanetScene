using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public interface ISpaceWindow
{

}

public class SpaceWindow : MonoBehaviour, ISpaceWindow, IStartable, IDisposable
{
    private List<IPlanetInteraction> planetUIs;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        planetUIs = gameObject.GetComponentsInChildren<IPlanetInteraction>().ToList();

        planetUIs.ForEach(planet => {resolver.Inject(planet) ; planet.Init(resolver); });
    }

    void IStartable.Start()
    {
        //
    }

    public void Initialize()
    {
        planetUIs.ForEach(planet => { planet.Enable(); });
    }

    public void Dispose()
    {
        planetUIs.ForEach(planet => { planet.Disable(); });
    }
}
