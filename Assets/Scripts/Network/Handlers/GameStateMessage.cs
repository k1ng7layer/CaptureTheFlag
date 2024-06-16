using GameResult;
using Mirror;

namespace Network.Handlers
{
    public struct GameStateMessage : NetworkMessage
    {
        public EGameState GameState;
    }
}