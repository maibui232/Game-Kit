namespace GameKit.Services.LocalData
{
    using System;
    using Cysharp.Threading.Tasks;
    using VContainer.Unity;

    public interface ILocalDataHandler : IStartable, IDisposable
    {
        UniTask SaveAllLocalData();
        UniTask LoadAllLocalData();
    }
}