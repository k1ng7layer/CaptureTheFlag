using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class MaterialSettings
    {
        public EColor ColorType;
        public Color Color;
    }
    
    [CreateAssetMenu(menuName = "settings/"+ nameof(PlayerColorSettings), fileName = nameof(PlayerColorSettings))]
    public class PlayerColorSettings : ScriptableObject
    {
        [SerializeField] private MaterialSettings[] _materials;

        public Color Get(EColor color)
        {
            foreach (var materialSettings in _materials)
            {
                if (materialSettings.ColorType == color)
                    return materialSettings.Color;
            }

            throw new Exception($"[{nameof(PlayerColorSettings)}] cant find material of color {color}");
        }
        
    }
}