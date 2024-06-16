using UI.Manager;
using Zenject;

namespace UI.Signals
{
    public static class SignalBusExtensions
    {
        public static void OpenWindow<T>(this SignalBus signalBus) where T : IUiWindow
        {
            signalBus.Fire(new SignalOpenWindow(typeof(T)));
        }
    }
}