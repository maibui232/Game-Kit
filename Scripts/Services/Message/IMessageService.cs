namespace GDK.Scripts.Services.Message
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using GDK.Scripts.Exception;
    using MessagePipe;
    using VContainer;

    public interface IMessageService
    {
        void Publish<TMessage>() where TMessage : new();
        void Publish<TMessage>(TMessage sender);
        void Subscribe<TMessage>(Action<TMessage> callback);
        void Subscribe<TMessage>(Action callback);
        void UnSubscribe<TMessage>(Action<TMessage> callback);
        void UnSubscribe<TMessage>(Action callback);
        void SendMessage<TKey, TMessage>(TKey key) where TMessage : new();
        void SendMessage<TKey, TMessage>(TKey key, TMessage sender) where TMessage : new();
        void Subscribe<TKey, TMessage>(TKey key, Action<TMessage> callback);
        void Subscribe<TKey, TMessage>(TKey key, Action callback);
        void UnSubscribe<TKey, TMessage>(TKey key, Action<TMessage> callback);
        void UnSubscribe<TKey, TMessage>(TKey key, Action callback);
    }

    public class MessageService : IMessageService
    {
        #region Inject

        private readonly IObjectResolver resolver;

        #endregion

        private readonly Dictionary<MethodInfo, IDisposable> methodInfoToDisposable = new();

        public MessageService(IObjectResolver resolver)
        {
            this.resolver = resolver;
            GlobalMessagePipe.SetProvider(resolver.AsServiceProvider());
        }

        public void Publish<T>() where T : new()
        {
            var publisher = this.resolver.Resolve<IPublisher<T>>();
            publisher.Publish(new T());
        }

        public void Publish<T>(T sender)
        {
            var publisher = this.resolver.Resolve<IPublisher<T>>();
            publisher.Publish(sender);
        }

        public void Subscribe<T>(Action<T> callback)
        {
            var bag        = DisposableBag.CreateBuilder();
            var subscriber = this.resolver.Resolve<ISubscriber<T>>();
            subscriber.Subscribe(obj => callback?.Invoke(obj)).AddTo(bag);

            var disposable = bag.Build();
            var methodInfo = callback.Method;

            if (this.methodInfoToDisposable.ContainsKey(methodInfo))
            {
                throw new GdkException($"Subscribe same message with same call back: {typeof(T)}");
            }

            this.methodInfoToDisposable.Add(callback.Method, disposable);
        }

        public void Subscribe<T>(Action callback)
        {
            var bag        = DisposableBag.CreateBuilder();
            var subscriber = this.resolver.Resolve<ISubscriber<T>>();
            subscriber.Subscribe(_ => callback?.Invoke()).AddTo(bag);

            var disposable = bag.Build();
            var methodInfo = callback.Method;
            if (this.methodInfoToDisposable.ContainsKey(methodInfo))
            {
                throw new GdkException($"Subscribe same message with same call back: {typeof(T)}");
            }

            this.methodInfoToDisposable.Add(methodInfo, disposable);
        }

        public void UnSubscribe<T>(Action<T> callback)
        {
            var methodInfo = callback.Method;
            if (!this.methodInfoToDisposable.TryGetValue(methodInfo, out var disposable))
            {
                throw new GdkException($"Callback {methodInfo.Name} does not define!");
            }

            disposable.Dispose();
            this.methodInfoToDisposable.Remove(methodInfo);
        }

        public void UnSubscribe<T>(Action callback)
        {
            var methodInfo = callback.Method;
            if (!this.methodInfoToDisposable.TryGetValue(methodInfo, out var disposable))
            {
                throw new GdkException($"Callback {methodInfo.Name} does not define!");
            }

            disposable.Dispose();
            this.methodInfoToDisposable.Remove(methodInfo);
        }

        public void SendMessage<TKey, TMessage>(TKey key) where TMessage : new()                  { }
        public void SendMessage<TKey, TMessage>(TKey key, TMessage sender) where TMessage : new() { }
        public void Subscribe<TKey, TMessage>(TKey key, Action<TMessage> callback)                { }
        public void Subscribe<TKey, TMessage>(TKey key, Action callback)                          { }
        public void UnSubscribe<TKey, TMessage>(TKey key, Action<TMessage> callback)              { }
        public void UnSubscribe<TKey, TMessage>(TKey key, Action callback)                        { }
    }
}