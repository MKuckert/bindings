using System.Diagnostics;

namespace Bindings.Core.Logging
{
    public class DebugWriteLineLogger : ILogger
    {
        public void Trace(string message, params object[] args)
        {
            Debug.WriteLine("Trace " + string.Format(message, args));
        }

        public void Warning(string message, params object[] args)
        {
            Debug.WriteLine("Warn " + string.Format(message, args));
        }

        public void Error(string message, params object[] args)
        {
            Debug.WriteLine("Error " + string.Format(message, args));
        }
    }
}