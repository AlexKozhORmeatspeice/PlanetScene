using Reptutation_Screen;
using VContainer;
using VContainer.Unity;

public class ReptutationLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<FractionSpawner>()
            .AsImplementedInterfaces();

        builder.RegisterComponentInHierarchy<DialogueReptutationManager>()
            .AsImplementedInterfaces()
            .AsSelf();
    }
}
