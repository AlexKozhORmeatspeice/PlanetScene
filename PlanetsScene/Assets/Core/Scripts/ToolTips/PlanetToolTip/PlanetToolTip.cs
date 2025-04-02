using UnityEngine;
using VContainer;

public class PlanetToolTip : MouseMoveToolTip
{
    public void Enable(IPlanetInfo planet)
    {
        toolTipPresenter.Enable(planet.Name, "", planet.Description);
    }
}