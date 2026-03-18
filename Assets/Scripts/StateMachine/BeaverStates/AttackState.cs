using System;
using AnimationHelper;
using Entites;
using UnityEngine;

namespace NewStateMachine.BeaverStates
{
    public class AttackState : IState<StateDataBase>
    {
        public event Action<StateDataBase> RequestToTransition;

        private bool _isAttacked;
        private Transform _owner;
        private Transform _targetTransform;
        private Animator _animator;
        private int _damage;
        private IDamagable _damagable;
        private AnimationEventReceiver _animReceiver;

        public AttackState(Transform owner, Animator animator, AnimationEventReceiver animReceiver, int damage)
        {
            _owner = owner;
            _animator = animator;
            _damage = damage;
            _animReceiver = animReceiver;
        }

        public void OnEnter(StateDataBase data)
        {
            if (data is AttackStateData stateData)
            {
                _damagable = stateData.Target;
                _targetTransform = stateData.TargetTransform;
            }

            _animReceiver.OnEvent += OnHit;
            _animator.SetBool("isAttack", true);
            _animator.Play("Attack");
        }

        public void FixedUpdate()
        {
            if ((Mathf.Abs(_owner.position.x - _targetTransform.position.x) > 0.5f))
            {
                RequestToTransition?.Invoke(new WalkStateData());
            }
        }

        public void OnExit()
        {
            _animReceiver.OnEvent -= OnHit;
            _animator.SetBool("isAttack", false);
        }

        private void OnHit()
        {
            if ((Mathf.Abs(_owner.position.x - _targetTransform.position.x) <= 0.5f))
            {
                _damagable.TakeDamage(_damage);
            }
        }
    }
}