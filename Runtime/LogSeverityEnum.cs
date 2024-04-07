using System;

namespace FeatureLogger.Runtime
{
    [Flags]
    public enum LogSeverityEnum
    {
        None = 0,
        Debug = 1 << 0,
        Warning = 1 << 1,
        Error = 1 << 2,
        Assert = 1 << 3
    }
}