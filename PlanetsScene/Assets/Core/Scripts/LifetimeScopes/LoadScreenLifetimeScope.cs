using Game_managers;
using Load_screen;
using VContainer;
using VContainer.Unity;

public class LoadScreenLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<LoadScreen>()
            .AsImplementedInterfaces();

        builder.Register<LoadManager>(Lifetime.Scoped)
            .AsImplementedInterfaces();
    }
}
