using VContainer;
using VContainer.Unity;
using Space_Screen;
using Planet_Window;
using Top_Bar;
using Menu;

public class MenuLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<MouseManager>(Lifetime.Scoped)
            .AsImplementedInterfaces();

        builder.RegisterComponentInHierarchy<MenuScreen>()
            .AsImplementedInterfaces()
            .AsSelf();
    }
}
