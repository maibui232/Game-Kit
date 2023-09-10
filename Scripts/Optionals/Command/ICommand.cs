namespace GameKit.Optionals.Command
{
    using System;
    using Cysharp.Threading.Tasks;

    public interface ICommand : IDisposable
    {
        void    Execute();
        UniTask UnDo();
    }
}