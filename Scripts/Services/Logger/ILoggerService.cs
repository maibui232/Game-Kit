namespace GDK.Scripts.Services.Logger
{
    using System;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public interface ILoggerService
    {
        void Log(string message);
        void Log(Object context);
        void Log(Color color, string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception exception);
        void LogAssert(bool condition);
        void LogAssert(bool condition, Object context);
        void LogAssert(bool condition, object context);
        void LogAssert(bool condition, object message, Object context);
    }

    public class LoggerService : ILoggerService
    {
        public void Log(string message) { Debug.Log(message); }

        public void Log(Object context) { Debug.Log(context); }

        public void Log(Color color, string message) { Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>"); }

        public void LogWarning(string message) { Debug.LogWarning(message); }

        public void LogError(string message) { Debug.LogError(message); }

        public void LogException(Exception exception) { Debug.LogException(exception); }

        public void LogAssert(bool condition) { Debug.Assert(condition); }

        public void LogAssert(bool condition, Object context) { Debug.Assert(condition, context); }

        public void LogAssert(bool condition, object context) { Debug.Assert(condition, context); }

        public void LogAssert(bool condition, object message, Object context) { Debug.Assert(condition, message, context); }
    }
}