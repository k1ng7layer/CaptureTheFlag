using System.Collections.Generic;
using Entitites;
using Settings;

namespace Services.Flags
{
    public interface IFlagsService
    {
        IReadOnlyDictionary<EColor, FlagEntity> Flags { get; }
        void SpawnFlag(EColor color);
    }
}