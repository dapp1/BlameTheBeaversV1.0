using UnityEngine;

namespace Configs.Root
{
    [CreateAssetMenu(menuName = "Configs/Root", fileName = "RootConfig", order = 1)]
    public class RootConfig : ScriptableObject
    {
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public Vector2 BeaversSpawnRangeSeconds { get; private set; }
        [field: SerializeField] public Vector2 GrowRangeSeconds { get; private set; }
        [field: SerializeField] public float RootInitialHealth { get; private set; }
        [field: SerializeField] public float LevelUpRootHealing { get; private set; }
        [field: SerializeField] public int HouseDamage { get; private set; }
    }
}