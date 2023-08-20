namespace GameKit.Services.LocalData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameKit.Extensions;
    using GameKit.Services.LocalData.ApplicationManager;
    using GameKit.Services.Logger;
    using GameKit.Services.Message;
    using Newtonsoft.Json;
    using UnityEngine;

    public class LocalDataHandler : ILocalDataHandler
    {
        #region Inject

        private readonly IMessageService              messageService;
        private readonly ILoggerService               loggerService;
        private readonly Dictionary<Type, ILocalData> typeToLocalData;

        #endregion

        private const string LocalDataPrefixKey = "LD_";

        public LocalDataHandler(IEnumerable<ILocalData> listLocalData, IMessageService messageService, ILoggerService loggerService)
        {
            this.messageService  = messageService;
            this.loggerService   = loggerService;
            this.typeToLocalData = listLocalData.ToDictionary(x => x.GetType(), x => x);
        }

        public async void Start()
        {
            this.messageService.Subscribe<ApplicationStateChangeMessage>(this.OnApplicationStateChange);
            await this.LoadAllLocalData();
        }

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

        private UniTask Load(Type type)
        {
            if (!PlayerPrefs.HasKey(this.Key(type)))
            {
                var data = Activator.CreateInstance(type);
                LoadData(data);
                return UniTask.CompletedTask;
            }

            var jsonData    = PlayerPrefs.GetString(this.Key(type));
            var convertData = JsonConvert.DeserializeObject(jsonData, type);
            LoadData(convertData);
            return UniTask.CompletedTask;

            void LoadData(object data)
            {
                this.typeToLocalData[data.GetType()] = data as ILocalData;
                this.loggerService.Log(Color.cyan, $"Load: {this.Key(type)}");
            }
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

        public async UniTask LoadAllLocalData()
        {
            var listTask = Enumerable.Select(ReflectionExtension.GetAllNonAbstractDerivedTypeFrom<ILocalData>(), this.Load).ToList();
            await UniTask.WhenAll(listTask);
            this.messageService.SendMessage<LoadLocalDataCompletedMessage>();
        }
    }
}