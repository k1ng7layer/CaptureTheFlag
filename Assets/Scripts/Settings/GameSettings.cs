using UnityEngine;
using UnityEngine.Serialization;

namespace Settings
{
    [CreateAssetMenu(menuName = "settings/"+ nameof(GameSettings), fileName = nameof(GameSettings))]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private int _flagsNumberPerPlayer;
        [SerializeField] private int _requiredPlayers = 3;

        public int FlagsNumPerPlayer => _flagsNumberPerPlayer;
        public int RequiredPlayers => _requiredPlayers;
    }
}