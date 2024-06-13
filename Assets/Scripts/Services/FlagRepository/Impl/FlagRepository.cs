using System;
using System.Collections.Generic;
using Entitites;
using Settings;

namespace Services.FlagRepository.Impl
{
    public class FlagRepository : IFlagRepository
    {
        private readonly Dictionary<EColor, List<FlagEntity>> _flagsEntities = new();
        
        public IReadOnlyDictionary<EColor, List<FlagEntity>> Flags => _flagsEntities;

        public event Action<FlagEntity> Added; 

        public void Add(FlagEntity flagEntity)
        {
            if (!_flagsEntities.ContainsKey(flagEntity.Color))
                _flagsEntities.Add(flagEntity.Color, new List<FlagEntity>());
            
            _flagsEntities[flagEntity.Color].Add(flagEntity);
            
            Added?.Invoke(flagEntity);
        }

        public void Remove(FlagEntity flagEntity)
        {
            _flagsEntities[flagEntity.Color].Remove(flagEntity);
        }
    }
}