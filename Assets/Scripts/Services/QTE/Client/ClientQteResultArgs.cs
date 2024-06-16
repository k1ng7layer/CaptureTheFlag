using Settings;
using UI.QteResult;

namespace Services.QTE.Client
{
    public readonly struct ClientQteResultArgs
    {
        public readonly EColor Player;
        public readonly EQteResult Result;

        public ClientQteResultArgs(EColor failedPlayer, EQteResult result)
        {
            Player = failedPlayer;
            Result = result;
        }
    }
}