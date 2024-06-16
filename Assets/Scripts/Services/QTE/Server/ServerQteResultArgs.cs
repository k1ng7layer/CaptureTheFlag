using UI.QteResult;

namespace Services.QTE.Server
{
    public readonly struct ServerQteResultArgs
    {
        public readonly int ConnectionId;
        public readonly EQteResult Result;

        public ServerQteResultArgs(int connectionId, EQteResult result)
        {
            ConnectionId = connectionId;
            Result = result;
        }
    }
}