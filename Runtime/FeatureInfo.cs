using System;
using UnityEngine;

namespace FeatureLogger.Runtime
{
    [Serializable]
    public class FeatureInfo
    {
        public bool Enabled;
        public string Name;
        public LogSeverityEnum Severity;
        public Color Color = Color.white;
    }
}

