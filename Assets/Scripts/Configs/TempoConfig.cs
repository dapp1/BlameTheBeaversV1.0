using UnityEngine;

namespace Configs
{
    [CreateAssetMenu (menuName = "Configs/Tempo" , fileName = "TempoConfig")]
    public class TempoConfig : ScriptableObject
    {
        [field: SerializeField] public int NeededScore { get; private set; }
        [field: SerializeField] public AnimationCurve TempoCurve { get; private set; }
        [field: SerializeField] public int MaxBeaverCount { get; private set; }
    }
}