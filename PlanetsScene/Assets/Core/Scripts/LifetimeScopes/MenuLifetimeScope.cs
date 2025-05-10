using VContainer;
using VContainer.Unity;
using Space_Screen;
using Planet_Window;
using Top_Bar;
using Menu;
using Save_screen;
using Settings_Screen;
using Load_screen;
using Game_managers;
using Game_camera;
using Frontier_anim;
using Frontier_UI;

public class MenuLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<MouseManager>(Lifetime.Scoped)
            .AsImplementedInterfaces();

        builder.Register<TextButtonMouseEvents>(Lifetime.Scoped) 
            .AsImplementedInterfaces();
        builder.Register<IconButtonMouseEvents>(Lifetime.Scoped)
            .AsImplementedInterfaces();
        builder.Register<SettingsSlotMouseEvents>(Lifetime.Scoped)
            .AsImplementedInterfaces();

        builder.Register<SlotMouseEvents>(Lifetime.Scoped)
            .AsImplementedInterfaces();
        builder.Register<NewSlotMouseEvents>(Lifetime.Scoped)
            .AsImplementedInterfaces();

        builder.Register<LoadManager>(Lifetime.Scoped) 
            .AsImplementedInterfaces();

        builder.Register<SavePopUpsManager>(Lifetime.Scoped)
            .AsImplementedInterfaces();

        builder.Register<SaveManager>(Lifetime.Scoped)
            .AsImplementedInterfaces();

        builder.RegisterComponentInHierarchy<MenuScreen>()
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<SaveScreen>()
            .AsImplementedInterfaces();

        builder.RegisterComponentInHierarchy<SettingsScreen>()
            .AsImplementedInterfaces();

        builder.RegisterComponentInHierarchy<MenuLoadScreen>()
            .AsImplementedInterfaces();

        builder.RegisterComponentInHierarchy<CameraView>()
            .AsImplementedInterfaces();

        builder.RegisterComponentInHierarchy<SaveSlotsManager>()
            .AsImplementedInterfaces()
            .AsSelf();
    }
}
