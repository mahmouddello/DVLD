using System;
using System.Diagnostics;

namespace DVLD.Infrastructure
{
    public static class Logger
    {
        public static void Log(string message, EventLogEntryType type, Exception ex = null, string context = null)
        {
            string fullMessage = context != null ? $"{context} : {message}" : message;

            if (ex != null)
                fullMessage += $" | Exception: {ex.Message}";

            if (!EventLog.SourceExists(Shared.SOURCE_NAME))
                EventLog.CreateEventSource(Shared.SOURCE_NAME, "Application");

            try
            {
                EventLog.WriteEntry(Shared.SOURCE_NAME, fullMessage, type);
            }
            catch 
            {
                // Fallback
                Console.WriteLine(fullMessage);
            }
        }
    }
}
