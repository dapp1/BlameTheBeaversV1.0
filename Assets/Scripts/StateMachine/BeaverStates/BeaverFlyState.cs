using System;

namespace NewStateMachine.BeaverStates
{
    public class BeaverFlyState : IState
    {
        public event Action<StateDataBase> RequestToTransition;
        public void OnEnter()
        {
            throw new NotImplementedException();
        }
        
        public void FixedUpdate()
        {
            throw new NotImplementedException();
        }

        public void OnExit()
        {
            throw new NotImplementedException();
        }
    }
}