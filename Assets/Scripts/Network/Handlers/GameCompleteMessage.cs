using Mirror;
using Settings;

namespace Services.Network.Handlers
{
    public struct GameCompleteMessage : NetworkMessage
    {
        public EColor Winner;
    }
}