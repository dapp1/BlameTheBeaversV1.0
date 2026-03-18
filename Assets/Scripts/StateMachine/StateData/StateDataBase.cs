using System;

namespace NewStateMachine
{
    public class StateDataBase
    {
        public StateType Type;

        public StateDataBase(StateType type) => Type = type;
    }
}