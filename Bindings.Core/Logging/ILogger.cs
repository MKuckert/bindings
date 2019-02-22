namespace Bindings.Core.Logging
{
    public interface ILogger
    {
        void Trace(string message, params object[] args);
        void Warning(string message, params object[] args);
        void Error(string message, params object[] args);
    }
}