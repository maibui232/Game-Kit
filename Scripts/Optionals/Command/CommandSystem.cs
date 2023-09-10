namespace GameKit.Optionals.Command
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GameKit.Services.Logger;
    using UnityEngine;

    public class CommandSystem
    {
        #region Inject

        private readonly ILoggerService loggerService;

        #endregion

        #region Cache

        private readonly List<ICommand>             commandList      = new();
        private readonly TaskCompletionSource<bool> isSetStackAmount = new(false);
        private          int                        maxStack;

        #endregion

        public CommandSystem(ILoggerService loggerService) { this.loggerService = loggerService; }

        public void SetMaxStackCommand(int maxCommand)
        {
            this.maxStack = maxCommand;
            this.isSetStackAmount.TrySetResult(true);
        }

        public void InvokeCommand(ICommand command)
        {
            command.Execute();
            this.commandList.Add(command);
            if (this.commandList.Count <= this.maxStack) return;

            var firstCommand = this.commandList[0];
            this.commandList.Remove(firstCommand);
            firstCommand.Dispose();
        }

        public async void InvokeUndo()
        {
            if (this.commandList.Count == 0)
            {
                this.loggerService.Log(Color.blue, "Can't undo");
                return;
            }

            var lastCommand = this.commandList.Last();
            this.commandList.Remove(lastCommand);
            await lastCommand.UnDo();
            lastCommand.Dispose();
        }
    }
}