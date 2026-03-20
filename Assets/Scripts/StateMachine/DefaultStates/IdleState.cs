using System;

namespace NewStateMachine.BeaverStates
{
    public class IdleState : IState<StateDataBase>
    {
        public event Action<StateDataBase> RequestToTransition;
        
        public void OnEnter(StateDataBase data)
        {
            
        }

        public void FixedUpdate()
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}