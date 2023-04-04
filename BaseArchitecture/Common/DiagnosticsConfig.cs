using System.Diagnostics;

namespace Common;

public static class DiagnosticsConfig
{
    public const string ServiceName = "API";
    public static ActivitySource ActivitySource = new(ServiceName);
}
