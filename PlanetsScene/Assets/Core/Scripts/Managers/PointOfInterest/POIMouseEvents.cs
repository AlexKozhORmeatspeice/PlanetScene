
public interface IPOIMouseEvents : IMouseEventsWithObject<IPointOfInterest>
{ }
public class POIMouseEvents : MouseEventsWithObject<IPointOfInterest>, IPOIMouseEvents
{ }