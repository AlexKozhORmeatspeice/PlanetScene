using UnityEngine;
using VContainer;

public class PlanetToolTip : MouseMoveToolTip
{
    public void Enable(IPlanet planet)
    {
        toolTipPresenter.Enable(planet.Name, "", planet.Description);
    }
}