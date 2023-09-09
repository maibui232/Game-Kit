namespace GameKit.Optionals.StateMachine
{
    using System;

    public interface IStateMachine
    {
        void OnTransition(Type nextState);
    }
}