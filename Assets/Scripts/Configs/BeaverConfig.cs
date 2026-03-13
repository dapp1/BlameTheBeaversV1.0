using UnityEngine;

namespace Configs.Beaver
{
    [CreateAssetMenu (menuName = "Configs/Beaver" , fileName = "BeaverConfig", order = 2)]
    public class BeaverConfig : ScriptableObject
    {
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }
    }
}