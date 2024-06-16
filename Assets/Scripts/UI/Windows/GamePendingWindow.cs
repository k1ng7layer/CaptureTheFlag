using UI.GamePending;
using UI.Manager;

namespace UI.Windows
{
    public class GamePendingWindow : UiWindow
    {
        public override void Setup()
        {
            AddController<GamePendingController>();
        }
    }
}