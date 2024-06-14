using System;
using System.Collections.Generic;
using Entitites;

namespace Services.Player
{
    public class PlayerHandler
    {
        private readonly List<PlayerEntity> _players = new();
        
        public GameEntity LocalPlayerEntityEntity { get; private set; }
        public List<PlayerEntity> Players => _players;

        public void AddPlayer(PlayerEntity player)
        {
            if (player.IsLocalPlayer)
                LocalPlayerEntityEntity = player;
            
            _players.Add(player);
        }
    }
}