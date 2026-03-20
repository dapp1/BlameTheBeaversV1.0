using System;
using NewStateMachine;

namespace StateMachine
{
    public class RootIdleState : IState<StateDataBase>
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