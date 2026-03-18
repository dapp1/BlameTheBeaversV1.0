using System;

namespace NewStateMachine
{
    public interface IState
    {
        event Action<StateDataBase> RequestToTransition;
        void FixedUpdate();
        void OnExit();
    }

    public interface IState<TData> : IState where TData : StateDataBase
    {
        void OnEnter(TData data);
    }

}