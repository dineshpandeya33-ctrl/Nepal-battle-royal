using UnityEngine;

namespace NepalBattleRoyale.Player
{
    /// <summary>Data asset used by the future character-selection screen.</summary>
    [CreateAssetMenu(fileName = "CharacterDefinition",
        menuName = "Nepal Battle Royale/Character Definition")]
    public class PlayerCharacterDefinition : ScriptableObject
    {
        [SerializeField] private string characterId = "nepal-starter";
        [SerializeField] private string displayName = "Himalayan Scout";
        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private Sprite portrait;
        [SerializeField, TextArea] private string description;

        public string CharacterId => characterId;
        public string DisplayName => displayName;
        public GameObject CharacterPrefab => characterPrefab;
        public Sprite Portrait => portrait;
        public string Description => description;
    }
}
