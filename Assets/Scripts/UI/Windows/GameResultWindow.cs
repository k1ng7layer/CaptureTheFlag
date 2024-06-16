using UI.GameResult;
using UI.Manager;

namespace UI.Windows
{
    public class GameResultWindow : UiWindow
    {
        public override void Setup()
        {
            AddController<GameResultController>();
        }
    }
}