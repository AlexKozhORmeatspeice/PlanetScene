using VContainer;
using VContainer.Unity;
using Space_Screen;
using Planet_Window;
using Top_Bar;
using Frontier_UI;
using Space_TopBar;

public class TravelWindowLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<EntryPoint>();

        builder.RegisterComponentInHierarchy<TravelManager>()
            .AsImplementedInterfaces();

        builder.Register<IconButtonMouseEvents>(Lifetime.Scoped)
            .AsImplementedInterfaces();

        builder.Register<TextButtonMouseEvents>(Lifetime.Scoped)
            .AsImplementedInterfaces();

        builder.Register<MouseManager>(Lifetime.Singleton)
            .AsImplementedInterfaces();

        PlanetWindowInit(builder);

        SectorScreenInit(builder);

        TopPartWindowInit(builder);
    }

    private void SectorScreenInit(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<SpaceWindow>()
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<PlanetTooltipSystem>()
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<ToolTip>()
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<PlanetMouseEvents>()
            .AsImplementedInterfaces()
            .AsSelf();
    }

    private void TopPartWindowInit(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<TopBar>()
            .AsImplementedInterfaces()
            .AsSelf();
    }

    private void PlanetWindowInit(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<PlanetWindow>()
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

        builder.RegisterComponentInHierarchy<POITooltipSystem>()
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<POIMouseEvents>()
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<StartFadePanel>()
            .AsImplementedInterfaces();
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
