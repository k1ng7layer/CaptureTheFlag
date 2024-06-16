using System.Collections.Generic;
using Entitites;
using Settings;

namespace Services.FlagSpawn
{
    public interface IFlagSpawnService
    {
        IReadOnlyDictionary<EColor, List<FlagEntity>> Flags { get; }
        FlagEntity SpawnFlag(EColor color, int owner);
    }
}