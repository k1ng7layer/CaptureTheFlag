using System.Collections.Generic;
using Entitites;
using Settings;

namespace Services.Flags
{
    public interface IFlagSpawnService
    {
        IReadOnlyDictionary<EColor, List<FlagEntity>> Flags { get; }
        FlagEntity SpawnFlag(EColor color);
    }
}