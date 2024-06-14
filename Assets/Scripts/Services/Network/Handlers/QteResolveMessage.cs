using Mirror;
using Settings;

namespace Services.Network.Handlers
{
    public struct QteResolveMessage : NetworkMessage
    {
        public EColor PlayerTeam;
        public float Value;
    }
}