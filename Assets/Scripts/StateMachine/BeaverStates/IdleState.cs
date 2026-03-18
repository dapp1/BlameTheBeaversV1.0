using System;

namespace NewStateMachine.BeaverStates
{
    public class IdleState : IState<StateDataBase>
    {
        public event Action<StateDataBase> RequestToTransition;
        
        public void OnEnter(StateDataBase data)
        {
            var player = UnityEngine.Object.FindObjectOfType<CharacterController>();
            RequestToTransition?.Invoke(new StateDataBase(StateType.Idle));
        }

        public void FixedUpdate()
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}