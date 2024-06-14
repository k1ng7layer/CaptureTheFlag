using Mirror;

namespace Services.Network.Handlers
{
    public struct StartQteMessage : NetworkMessage
    {
        public float ZoneStart;
        public float ZoneWidth;
    }
}