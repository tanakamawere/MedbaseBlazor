using Microsoft.IdentityModel.Abstractions;

namespace MedbaseBlazor.Services;

public interface IIdentityLogger
{
    bool IsEnabled(EventLogLevel eventLogLevel);
    void Log(LogEntry entry);
}
