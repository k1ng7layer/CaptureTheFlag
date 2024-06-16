using Mirror;
using Settings;
using UI.QteResult;

namespace Services.Network.Handlers
{
    public struct QteResultMessage : NetworkMessage
    {
        public EQteResult Result;
        public EColor FailedTeam;
    }
}