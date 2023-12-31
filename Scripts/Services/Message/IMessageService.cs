namespace GameKit.Services.Message
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using GameKit.Exception;
    using MessagePipe;
    using VContainer;

    public interface IMessageService
    {
        void Send<TMessage>() where TMessage : new();
        void Send<TMessage>(TMessage message);
        void Subscribe<TMessage>(Action<TMessage> callback);
        void Subscribe<TMessage>(Action callback);
        void UnSubscribe<TMessage>(Action<TMessage> callback);
        void UnSubscribe<TMessage>(Action callback);
        void SendWithKey<TKey, TMessage>(TKey key) where TMessage : new();
        void SendWithKey<TKey, TMessage>(TKey key, TMessage message);
        void SubscribeWithKey<TKey, TMessage>(TKey key, Action<TMessage> callback);
        void SubscribeWithKey<TKey, TMessage>(TKey key, Action callback);
        void UnSubscribeWithKey<TKey, TMessage>(TKey key, Action<TMessage> callback);
        void UnSubscribeWithKey<TKey, TMessage>(TKey key, Action callback);
    }

    public class MessageService : IMessageService
    {
        #region Inject

        private readonly IObjectResolver resolver;

        #endregion

        private readonly Dictionary<MethodInfo, IDisposable>     messageToDisposable        = new();
        private readonly Dictionary<KeyMessagePair, IDisposable> keyMessagePairToDictionary = new();

        public MessageService(IObjectResolver resolver)
        {
            this.resolver = resolver;
            GlobalMessagePipe.SetProvider(resolver.AsServiceProvider());
        }

        public void Send<T>() where T : new()
        {
            var publisher = this.resolver.Resolve<IPublisher<T>>();
            publisher.Publish(new T());
        }

        public void Send<T>(T message)
        {
            var publisher = this.resolver.Resolve<IPublisher<T>>();
            publisher.Publish(message);
        }

        public void Subscribe<T>(Action<T> callback)
        {
            var methodInfo = callback.Method;
            if (this.messageToDisposable.TryGetValue(methodInfo, out _))
            {
                throw new GdkException($"Subscribe same message with same call back: {typeof(T)}");
            }

            var bag        = DisposableBag.CreateBuilder();
            var subscriber = this.resolver.Resolve<ISubscriber<T>>();
            subscriber.Subscribe(obj => callback?.Invoke(obj)).AddTo(bag);
            var disposable = bag.Build();
            this.messageToDisposable.Add(methodInfo, disposable);
        }

        public void Subscribe<T>(Action callback)
        {
            var methodInfo = callback.Method;
            if (this.messageToDisposable.TryGetValue(methodInfo, out _))
            {
                throw new GdkException($"Subscribe same message with same call back: {typeof(T)}");
            }

            var bag        = DisposableBag.CreateBuilder();
            var subscriber = this.resolver.Resolve<ISubscriber<T>>();
            subscriber.Subscribe(_ => callback()).AddTo(bag);
            var disposable = bag.Build();
            this.messageToDisposable.Add(methodInfo, disposable);
        }

        public void UnSubscribe<T>(Action<T> callback)
        {
            var methodInfo = callback.Method;
            if (!this.messageToDisposable.TryGetValue(methodInfo, out var disposable))
            {
                throw new GdkException($"Callback {methodInfo.Name} does not define!");
            }

            disposable.Dispose();
            this.messageToDisposable.Remove(methodInfo);
        }

        public void UnSubscribe<T>(Action callback)
        {
            var methodInfo = callback.Method;
            if (!this.messageToDisposable.TryGetValue(methodInfo, out var disposable))
            {
                throw new GdkException($"Callback {methodInfo.Name} does not define!");
            }

            disposable.Dispose();
            this.messageToDisposable.Remove(methodInfo);
        }

        public void SendWithKey<TKey, TMessage>(TKey key) where TMessage : new()
        {
            var publisher = this.resolver.Resolve<IPublisher<TKey, TMessage>>();
            publisher.Publish(key, new TMessage());
        }

        public void SendWithKey<TKey, TMessage>(TKey key, TMessage message)
        {
            var publisher = this.resolver.Resolve<IPublisher<TKey, TMessage>>();
            publisher.Publish(key, message);
        }

        public void SubscribeWithKey<TKey, TMessage>(TKey key, Action<TMessage> callback)
        {
            var keyMessagePair = new KeyMessagePair(key.GetHashCode(), callback.Method);
            if (this.keyMessagePairToDictionary.TryGetValue(keyMessagePair, out _))
            {
                throw new GdkException($"Subscribe same message with same call back: {typeof(TKey)},{typeof(TMessage)}");
            }

            var bag        = DisposableBag.CreateBuilder();
            var subscriber = this.resolver.Resolve<ISubscriber<TKey, TMessage>>();
            subscriber.Subscribe(key, callback).AddTo(bag);
            var disposable = bag.Build();
            this.keyMessagePairToDictionary.Add(keyMessagePair, disposable);
        }

        public void SubscribeWithKey<TKey, TMessage>(TKey key, Action callback)
        {
            var keyMessagePair = new KeyMessagePair(key.GetHashCode(), callback.Method);
            if (this.keyMessagePairToDictionary.TryGetValue(keyMessagePair, out _))
            {
                throw new GdkException($"Subscribe same message with same call back: {typeof(TKey)},{typeof(TMessage)}");
            }

            var bag        = DisposableBag.CreateBuilder();
            var subscriber = this.resolver.Resolve<ISubscriber<TKey, TMessage>>();
            subscriber.Subscribe(key, _ => callback()).AddTo(bag);
            var disposable = bag.Build();
            this.keyMessagePairToDictionary.Add(keyMessagePair, disposable);
        }

        public void UnSubscribeWithKey<TKey, TMessage>(TKey key, Action<TMessage> callback)
        {
            var keyMessagePair = new KeyMessagePair(key.GetHashCode(), callback.Method);
            if (!this.keyMessagePairToDictionary.TryGetValue(keyMessagePair, out var disposable))
            {
                throw new GdkException($"Callback {callback.Method.Name} does not define!");
            }

            disposable.Dispose();
            this.keyMessagePairToDictionary.Remove(keyMessagePair);
        }

        public void UnSubscribeWithKey<TKey, TMessage>(TKey key, Action callback)
        {
            var keyMessagePair = new KeyMessagePair(key.GetHashCode(), callback.Method);
            if (!this.keyMessagePairToDictionary.TryGetValue(keyMessagePair, out var disposable))
            {
                throw new GdkException($"Callback {callback.Method.Name} does not define!");
            }

            disposable.Dispose();
            this.keyMessagePairToDictionary.Remove(keyMessagePair);
        }
    }

    public struct KeyMessagePair
    {
        public int        HashKey;
        public MethodInfo MethodInfo;

        public KeyMessagePair(int hashKey, MethodInfo methodInfo)
        {
            this.HashKey    = hashKey;
            this.MethodInfo = methodInfo;
        }
    }
}