using Mirror;

namespace Services.Network.Handlers
{
    public struct SpawnPlayerMessage : NetworkMessage
    {
        public int Player;
    }
}