

using Game_UI;

public interface IPlanetOrbitObserver
{
    void Enable();
}

public class PlanetOrbitObserver : IPlanetOrbitObserver 
{
    private IRotatingObject view;

    public PlanetOrbitObserver(IRotatingObject view)
    {
        this.view = view;
    }

    public void Enable()
    {
        view.StartAnim();
    }
}