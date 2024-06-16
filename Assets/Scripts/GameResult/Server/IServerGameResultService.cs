using System;
using Settings;

namespace GameResult.Server
{
    public interface IServerGameResultService
    {
        event Action<EColor> GameCompleted;
    }
}