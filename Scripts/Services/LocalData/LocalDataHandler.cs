namespace GameKit.Services.LocalData
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameKit.Services.LocalData.ApplicationManager;
    using GameKit.Services.Logger;
    using GameKit.Services.Message;
    using Newtonsoft.Json;
    using UnityEngine;
    using VContainer;

    public class LocalDataHandler : ILocalDataHandler
    {
        #region Inject

        private readonly IMessageService              messageService;
        private readonly ILoggerService               loggerService;
        private readonly IObjectResolver              objectResolver;
        private readonly Dictionary<Type, ILocalData> typeToLocalData = new();

        #endregion

        private const string LocalDataPrefixKey = "LD_";

        public LocalDataHandler(IMessageService messageService, ILoggerService loggerService)
        {
            this.messageService = messageService;
            this.loggerService  = loggerService;
        }

        public void Start() { this.messageService.Subscribe<ApplicationStateChangeMessage>(this.OnApplicationStateChange); }

        public void Dispose() { this.messageService.UnSubscribe<ApplicationStateChangeMessage>(this.OnApplicationStateChange); }

        private async void OnApplicationStateChange(ApplicationStateChangeMessage obj)
        {
            if (obj.state is not (ApplicationState.Pause or ApplicationState.Quit)) return;
            await this.SaveAllLocalData();
        }

        private string Key(Type type) => $"{LocalDataPrefixKey}{type.Name}";

        private UniTask Save(Type type)
        {
            var localData = this.typeToLocalData[type];
            var jsonData  = JsonConvert.SerializeObject(localData);
            PlayerPrefs.SetString(this.Key(type), jsonData);
            Debug.Log(jsonData);
            this.loggerService.Log(Color.green, $"Save: {this.Key(type)}");
            return UniTask.CompletedTask;
        }

        public T Load<T>() where T : ILocalData, new()
        {
            if (!PlayerPrefs.HasKey(this.Key(typeof(T))))
            {
                var data = new T();
                this.typeToLocalData.Add(typeof(T), data);
                return data;
            }

            var jsonData  = PlayerPrefs.GetString(this.Key(typeof(T)));
            var localData = JsonConvert.DeserializeObject<T>(jsonData);
            this.typeToLocalData.Add(typeof(T), localData);
            return localData;
        }

        public UniTask SaveAllLocalData()
        {
            var listTask = new List<UniTask>();
            foreach (var (type, _) in this.typeToLocalData)
            {
                var task = this.Save(type);
                listTask.Add(task);
            }

            return UniTask.WhenAll(listTask);
        }
    }
}