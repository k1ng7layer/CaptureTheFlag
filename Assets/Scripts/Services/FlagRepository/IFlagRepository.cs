using System;
using System.Collections.Generic;
using Entitites;
using Settings;

namespace Services.FlagRepository
{
    public interface IFlagRepository
    {
        IReadOnlyDictionary<EColor, List<FlagEntity>> Flags { get; }

        event Action<FlagEntity> Added;
        void Add(FlagEntity flagEntity);
        void Remove(FlagEntity flagEntity);
    }
}