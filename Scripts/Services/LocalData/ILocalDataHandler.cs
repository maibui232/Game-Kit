namespace GameKit.Services.LocalData
{
    using System;
    using Cysharp.Threading.Tasks;
    using VContainer.Unity;

    public interface ILocalDataHandler : IStartable, IDisposable
    {
        UniTask    SaveAllLocalData();
        T Load<T>() where T : ILocalData, new();
    }
}