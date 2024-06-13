using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "settings/"+ nameof(GameSettings), fileName = nameof(GameSettings))]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private int _flagsNumberPerPlayer;

        public int FlagsNumPerPlayer => _flagsNumberPerPlayer;
    }
}