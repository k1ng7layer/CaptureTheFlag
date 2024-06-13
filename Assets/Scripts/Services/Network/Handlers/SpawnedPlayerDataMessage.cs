using Mirror;
using Settings;

namespace Services.Network.Handlers
{
    public struct SpawnedPlayerDataMessage : NetworkMessage
    {
        public EColor AssignedColor;
    }
}