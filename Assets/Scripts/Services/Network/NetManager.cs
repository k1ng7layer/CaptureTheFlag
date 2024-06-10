using System;
using Mirror;
using Services.Network.Handlers;
using UnityEngine;

namespace Services.Network
{
    public class NetManager : NetworkManager
    {
        public event Action ClientConnected;
        
        public override void OnClientConnect()
        {
            Debug.Log($"NetManager OnClientConnect");
            base.OnClientConnect();

            NetworkClient.Send(new SpawnPlayerMessage());
           // SpawnPlayer();
        }

        public override void OnStartServer()
        {
            Debug.Log($"NetManager OnStartServer");
            base.OnStartServer();
            
            //NetworkServer.RegisterHandler<SpawnPlayerMessage>(Spawn);
        }

        private void Spawn(NetworkConnectionToClient conn, SpawnPlayerMessage msg, int chId)
        {
            // var player = Instantiate(playerPrefab);
            // //     
            // NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}