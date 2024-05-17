using Microsoft.IdentityModel.Abstractions;

namespace MedbaseBlazor.Services;

class MedbaseIdentityLogger : IIdentityLogger
{
    public EventLogLevel MinLogLevel { get; }

    public MedbaseIdentityLogger()
    {
        //Recommended default log level
        MinLogLevel = EventLogLevel.LogAlways;
    }

    public bool IsEnabled(EventLogLevel eventLogLevel)
    {
        return eventLogLevel <= MinLogLevel;
    }

    public void Log(LogEntry entry)
    {
        //Log Message here:
        Console.WriteLine(entry.Message);
    }
}
