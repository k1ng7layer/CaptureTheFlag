using System;
using Mirror;
using Services.Network.Handlers;

namespace Services.Network
{
    public class NetManager : NetworkManager
    {
        public event Action ClientConnectedToServer;
        
        public override void OnClientConnect()
        {
            base.OnClientConnect();
       
            ClientConnectedToServer?.Invoke();
            NetworkClient.Send(new PlayerReadyMessage());
        }
    }
}