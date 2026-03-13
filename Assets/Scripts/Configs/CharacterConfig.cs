using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Configs.Charactert
{
    [CreateAssetMenu (menuName = "Configs/Character" , fileName = "CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [field: SerializeField] public float CharacterSpeed { get; private set; }
        [field: SerializeField] public int JumpForce { get; private set; }
        [field: SerializeField] public float FreezeDurationSeconds { get; private set; }
    }
}
