namespace GameKit.Optionals.StateMachine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameKit.Services.Logger;
    using GameKit.Services.Message;
    using UnityEngine;

    public abstract class BaseStateMachine<T> : IStateMachine where T : IState
    {
        #region Inject

        private readonly   ILoggerService      loggerService;
        protected readonly IMessageService     MessageService;
        private readonly   Dictionary<Type, T> typeToState;

        #endregion

        public T CurrentState { get; private set; }

        protected BaseStateMachine(IEnumerable<T> states, ILoggerService loggerService, IMessageService messageService)
        {
            this.loggerService  = loggerService;
            this.MessageService = messageService;
            this.typeToState    = states.ToDictionary(x => x.GetType(), x => x);
        }

        public async void OnTransition(Type nextState)
        {
            if (this.CurrentState != null)
            {
                await this.CurrentState.Exit();
            }

            if (!this.typeToState.TryGetValue(nextState, out var state))
            {
                this.loggerService.Log(Color.red, $"Does not contain state: {nextState.Name}");
                return;
            }

            state.Enter();
            this.CurrentState = state;
        }
    }
}