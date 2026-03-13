using UnityEngine;

namespace Configs.General
{
    [CreateAssetMenu (menuName = "Configs/General" , fileName = "GeneralConfig")]
    public class GeneralConfig : ScriptableObject
    {
        [field: SerializeField] public int InitialCoins { get; private set; }
        [field: SerializeField] public int CoinsForBeaver { get; private set; }
        [field: SerializeField] public int CoinsForRoot { get; private set; }
        [field: SerializeField] public int ScoreForBeaver { get; private set; }
        [field: SerializeField] public int ScoreForRoot { get; private set; }
        [field: SerializeField] public int ScoreForSecond { get; private set; }
    }
}