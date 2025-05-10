
namespace Load_screen
{
    public interface ICrystalBgObserver
    {
        void Enable();
    }

    public class CrystalObserver : ICrystalBgObserver
    {
        private IPulsObj view;
        
        public CrystalObserver(IPulsObj view)
        {
            this.view = view;
        }

        public void Enable()
        {
            view.StartAnim();
        }
    }
}
