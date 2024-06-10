using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class MaterialSettings
    {
        public EColor Color;
        public Material Material;
    }
    
    [CreateAssetMenu(menuName = "settings/"+ nameof(PlayerColorSettings), fileName = nameof(PlayerColorSettings))]
    public class PlayerColorSettings : ScriptableObject
    {
        [SerializeField] private MaterialSettings[] _materials;

        public Material Get(EColor color)
        {
            foreach (var materialSettings in _materials)
            {
                if (materialSettings.Color == color)
                    return materialSettings.Material;
            }

            throw new Exception($"[{nameof(PlayerColorSettings)}] cant find material of color {color}");
        }
        
    }
}