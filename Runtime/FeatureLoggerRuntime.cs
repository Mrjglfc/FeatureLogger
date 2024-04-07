using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FeatureLogger.Runtime
{
    public class FeatureLoggerRuntime : MonoBehaviour
    {
        [SerializeField] private FeatureInfoContainer featureInfoContainer;

        private static Dictionary<string, FeatureInfo> features;

        private void Awake()
        {
            // Null & bounds check to make sure container has data inside.
            if (featureInfoContainer != null && featureInfoContainer.featureInfos.Length > 0)
            {
                // Convert to dictionary as logger runtime will need to do fast lookup.
                // Also only pulls values which have been marked as "Enabled".
                features = featureInfoContainer.featureInfos.Where(x=> x.Enabled).ToDictionary(x => x.Name, x => x);
            }
            else
            {
                Debug.LogError("[FeatureLogger] FeatureInfoContainer is null! Please fill in the object reference");
            }
        }

        #region Debug Logs
        public static void Log(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Debug))
            {
                Debug.Log($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color)}");
            }
        }

        public static void Log(string label, string message, GameObject context)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Debug))
            {
                Debug.Log($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color)}", context);
            }
        }
        #endregion

        #region Warning Logs
        public static void LogWarning(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Warning))
            {
                Debug.LogWarning($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color)}");
            }
        }

        public static void LogWarning(string label, string message, GameObject context)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Warning))
            {
                Debug.LogWarning($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color)}", context);
            }
        }
        #endregion

        #region Error Logs
        public static void LogError(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Error))
            {
                Debug.LogError($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color)}");
            }
        }

        public static void LogError(string label, string message, GameObject context)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Error))
            {
                Debug.LogError($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color)}", context);
            }
        }

        #endregion

        #region Assertion Logs
        public static void LogAssertion(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Assert))
            {
                Debug.LogAssertion($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color)}");
            }
        }

        public static void LogAssertion(string label, string message, GameObject context)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Assert))
            {
                Debug.LogAssertion($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color)}", context);
            }
        }

        #endregion
    }
}