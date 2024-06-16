using System;
using Settings;

namespace GameResult.Client
{
    public interface IClientGameResultService
    {
        event Action<EColor> GameCompleted;
    }
}