using Entitites;
using Settings;

namespace Services.Player
{
    public interface IPlayerSpawnService
    {
        GameEntity SpawnPlayer(int connectionId, EColor color);
    }
}