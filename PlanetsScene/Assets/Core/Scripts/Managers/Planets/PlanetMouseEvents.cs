
public interface IPlanetMouseEvents : IMouseEventsWithObject<IPlanet>
{

}
public class PlanetMouseEvents : MouseEventsWithObject<IPlanet>, IPlanetMouseEvents
{ 

}
