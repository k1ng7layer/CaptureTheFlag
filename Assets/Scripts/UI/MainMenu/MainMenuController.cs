using System;
using Mirror;
using Services.Network;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuController : UiController<MainMenuView>,
        IInitializable,
        IDisposable
    {
        private readonly NetManager _netManager;

        public MainMenuController(NetManager netManager)
        {
            _netManager = netManager;
        }
        
        public void Initialize()
        {
            View.Initialize(HostGame, JoinGame);
            _netManager.ClientConnectedToServer += OnConnectedToServer;
        }
        
        public void Dispose()
        {
            _netManager.ClientConnectedToServer -= OnConnectedToServer;
        }

        private void HostGame()
        {
            NetworkManager.singleton.StartHost();
        }

        private void JoinGame()
        {
            NetworkManager.singleton.StartClient();
        }
        
        private void OnConnectedToServer()
        {
            View.Hide();
        }
    }
}