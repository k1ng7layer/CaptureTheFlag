﻿using System;
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
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
        }
    }
}