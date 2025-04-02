using VContainer;
using VContainer.Unity;

public class CustomLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<EntryPoint>();

        builder.Register<TravelManager>(Lifetime.Scoped)
            .AsImplementedInterfaces();

        builder.Register<MouseManager>(Lifetime.Singleton)
            .AsImplementedInterfaces();

        SectorWindowInit(builder);

        PlanetWindowInit(builder);

        TopPartWindowInit(builder);
    }

    private void SectorWindowInit(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<SpaceWindow>()
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<PlanetToolTip>()
            .AsImplementedInterfaces()
            .AsSelf();
    }

    private void TopPartWindowInit(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<TopPartWindow>()
            .AsImplementedInterfaces()
            .AsSelf();
    }

    private void PlanetWindowInit(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<PlanetWindow>()
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<POIToolTip>()
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<PlanetWindow_ScanerButton>()
            .AsImplementedInterfaces();

        Points(builder);

        builder.RegisterComponentInHierarchy<Scaner>()
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<Drone>()
            .AsImplementedInterfaces();

        builder.RegisterComponentInHierarchy<NotFound>()
            .AsImplementedInterfaces();

        builder.RegisterComponentInHierarchy<PointOfInterestGraphic>()
            .AsImplementedInterfaces()
            .AsSelf();
    }

    private void Points(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<PointOfInterest>()
                .AsImplementedInterfaces()
                .AsSelf();

        builder.RegisterComponentInHierarchy<PointsSpawner>()
           .AsImplementedInterfaces()
           .AsSelf();

        builder.Register<PointOfInterestFactory>(Lifetime.Scoped)
            .AsImplementedInterfaces();
    }
}
