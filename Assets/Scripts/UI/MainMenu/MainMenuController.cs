using System;
using Mirror;
using Services.Network;
using UI.Manager;
using UI.Signals;
using UI.Windows;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuController : UiController<MainMenuView>,
        IInitializable,
        IDisposable
    {
        private readonly NetManager _netManager;
        private readonly SignalBus _signalBus;

        public MainMenuController(NetManager netManager, SignalBus signalBus)
        {
            _netManager = netManager;
            _signalBus = signalBus;
        }

        public void Dispose()
        {
            _netManager.ClientConnectedToServer -= OnConnectedToServer;
        }

        public void Initialize()
        {
            View.Initialize(HostGame, JoinGame);
            _netManager.ClientConnectedToServer += OnConnectedToServer;
        }

        private void HostGame()
        {
            NetworkManager.singleton.StartHost();
            
            _signalBus.OpenWindow<GamePendingWindow>();
        }

        private void JoinGame()
        {
            NetworkManager.singleton.StartClient();
            _signalBus.OpenWindow<GamePendingWindow>();
        }

        private void OnConnectedToServer()
        {
            View.Hide();
        }
    }
}