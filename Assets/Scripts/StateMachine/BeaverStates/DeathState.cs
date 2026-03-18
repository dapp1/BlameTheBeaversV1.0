using System;
using AnimationHelper;
using UnityEngine;

namespace NewStateMachine.BeaverStates
{
    public class DeathState : IState<StateDataBase>
    {
        private GameObject _owner;
        private Animator _animator;
        private AnimationEventReceiver _receiver;

        public event Action<StateDataBase> RequestToTransition;

        public DeathState(Animator animator, AnimationEventReceiver receiver, GameObject owner)
        {
            _owner = owner;
            _animator = animator;
            _receiver = receiver;
        }

        public void OnEnter(StateDataBase data)
        {
            _animator.Play("Death");

            _receiver.OnEvent += OnDie;
        }

        public void FixedUpdate()
        {
            
        }

        public void OnExit()
        {
            _owner.SetActive(false);
        }

        private void OnDie()
        {
            _owner.SetActive(false);
            _receiver.OnEvent -= OnDie;
            RequestToTransition?.Invoke(new StateDataBase(StateType.Idle));
        }
    }
}