using UI.Input;
using UI.Manager;
using UI.QTE;
using UI.QteResult;

namespace UI.Windows
{
    public class GameHudWindow : UiWindow
    {
        public override void Setup()
        {
            AddController<QteController>();
            AddController<QteResultController>();
            AddController<InputController>();
        }
    }
}