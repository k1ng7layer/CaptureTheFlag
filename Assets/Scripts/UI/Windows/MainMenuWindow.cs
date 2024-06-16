using UI.MainMenu;
using UI.Manager;

namespace UI.Windows
{
    public class MainMenuWindow : UiWindow
    {
        public override void Setup()
        {
            AddController<MainMenuController>();
        }
    }
}