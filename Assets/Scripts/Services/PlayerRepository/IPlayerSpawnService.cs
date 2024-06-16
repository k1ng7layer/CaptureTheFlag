using Entitites;
using Settings;

namespace Services.PlayerRepository
{
    public interface IPlayerSpawnService
    {
        GameEntity SpawnPlayer(int connectionId, EColor color);
    }
}