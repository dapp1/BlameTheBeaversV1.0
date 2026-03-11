using UnityEngine;

namespace Configs.BeaverShip
{
    [CreateAssetMenu (menuName = "Configs/BeaverShip" , fileName = "BeaverShipConfig", order = 0)]
    public class BeaverShipConfig : ScriptableObject
    {
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public Vector2 SpeedRange { get; private set; }
     
        [field: SerializeField] public int RootChance { get; private set; }
        [field: SerializeField] public GameObject RootPrefab { get; private set; }
        
        [field: SerializeField] public int BeaverChance { get; private set; }
        [field: SerializeField] public GameObject BeaverPrefab { get; private set; }
    }
}