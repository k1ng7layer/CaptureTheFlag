using Mirror;
using Settings;

namespace Services.Network.Handlers
{
    public struct QteFailMessage : NetworkMessage
    {
        public EColor FailedTeam;
    }
}