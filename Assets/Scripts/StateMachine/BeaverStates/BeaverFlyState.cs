using System;
using UnityEngine;

namespace NewStateMachine.BeaverStates
{
    public class BeaverFlyState : IState<StateDataBase>
    {
        public event Action<StateDataBase> RequestToTransition;

        private Animator _animator;
        
        public BeaverFlyState(Animator animator)
        {
            _animator = animator;
        }

        public void OnEnter(StateDataBase data)
        {
            _animator.Play("Fly");
        }
        
        public void FixedUpdate()
        {
            
        }

        public void OnExit()
        {
        }
        
    }
}