using Entites;
using NewStateMachine.BeaverStates;
using UnityEngine;

namespace NewStateMachine
{
    public class AttackStateData : StateDataBase
    {
        public IDamagable Target;
        public Transform TargetTransform;

        public AttackStateData(IDamagable target, Transform targetTransform) : base(StateType.Attack)
        {
            Target = target;
            TargetTransform = targetTransform;
        }
    }
}