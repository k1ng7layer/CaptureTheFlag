using System;
using Settings;

namespace GameState
{
    public interface IGameResultService
    {
        event Action<EColor> GameCompleted;
    }
}