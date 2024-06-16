using UI.Manager;
using Zenject;

namespace UI.GamePending
{
    public class GamePendingController : UiController<GamePendingView>, 
        IInitializable
    {
        public void Initialize()
        {
            View.Hide();
        }

        protected override void OnShow()
        {
            View.SetPendingInfo($"Ожидание игроков");
        }
    }
}