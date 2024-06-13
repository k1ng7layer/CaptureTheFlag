using System;
using System.Collections.Generic;
using Entitites;

namespace Services.Player
{
    public class PlayerHandler
    {
        private readonly List<GameEntity> _players = new();
        
        public GameEntity LocalPlayerEntityEntity { get; private set; }
        public List<GameEntity> Players => _players;

        public void AddPlayer(GameEntity player)
        {
            if (player.IsLocalPlayer)
                LocalPlayerEntityEntity = player;
            
            _players.Add(player);
        }
    }
}