using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FeatureLogger.Runtime
{
    public class FeatureLoggerRuntime : MonoBehaviour
    {
        public FeatureInfoContainer featureInfoContainer;

        private static Dictionary<string, FeatureInfo> features;
        private static FeatureInfoContainer FeatureInfoContainer;

        private void Start()
        {
            // Null & bounds check to make sure container has data inside.
            if (featureInfoContainer == null)
            {
                Debug.LogError($"{HelperUtils.FEATURELOGGER_TAG} FeatureInfoContainer is null! Please fill in the inspector reference within the FeatureContainer ScriptableObject");
                return;
            }

            FeatureInfoContainer = featureInfoContainer;

            // Convert to dictionary as logger runtime will need to do fast lookup.
            // Also only pulls values which have been marked as "Enabled".
            features = FeatureInfoContainer.featureGroups.Where(x => x.Enabled).ToDictionary(x => x.Name, x => x);

            if (features.Count == 0 || features == null)
            {
                Debug.LogWarning($"{HelperUtils.FEATURELOGGER_TAG} There are no groups enabled or assigned within the FeatureContainer ScriptableObject.\n" +
                    $"FeatureLogger will not be working as expected");
            }
        }

        #region Debug Logs
        public static void Log(string label, string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Debug))
            {
                string finalisedMessage = HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, FeatureInfoContainer.colorEntireMessage);
                Debug.Log(message: $"{HelperUtils.GetCallerContext(file, line, member)} {finalisedMessage}");
            }
        }

        public static void Log(string label, string message, GameObject context,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Debug))
            {
                string finalisedMessage = HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, FeatureInfoContainer.colorEntireMessage);
                Debug.Log(message: $"{HelperUtils.GetCallerContext(file, line, member)} {finalisedMessage}", context);
            }
        }
        #endregion

        #region Warning Logs
        public static void LogWarning(string label, string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Warning))
            {
                Color color = FeatureInfoContainer.overrideConsoleStyling ? featureInfo.Color : Color.yellow;
                string finalisedMessage = HelperUtils.GetFinalisedMessage(label, message, color, FeatureInfoContainer.colorEntireMessage);
                Debug.LogWarning($"{HelperUtils.GetCallerContext(file, line, member)} {finalisedMessage}");
            }
        }

        public static void LogWarning(string label, string message, GameObject context,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Warning))
            {
                Color color = FeatureInfoContainer.overrideConsoleStyling ? featureInfo.Color : Color.yellow;
                string finalisedMessage = HelperUtils.GetFinalisedMessage(label, message, color, FeatureInfoContainer.colorEntireMessage);
                Debug.LogWarning($"{HelperUtils.GetCallerContext(file, line, member)} {finalisedMessage}", context);
            }
        }
        #endregion

        #region Error Logs
        public static void LogError(string label, string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Error))
            {
                Color color = FeatureInfoContainer.overrideConsoleStyling ? featureInfo.Color : Color.red;
                string finalisedMessage = HelperUtils.GetFinalisedMessage(label, message, color, FeatureInfoContainer.colorEntireMessage);
                Debug.LogError($"{HelperUtils.GetCallerContext(file, line, member)} {finalisedMessage}");
            }
        }

        public static void LogError(string label, string message, GameObject context,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Error))
            {
                Color color = FeatureInfoContainer.overrideConsoleStyling ? featureInfo.Color : Color.red;
                string finalisedMessage = HelperUtils.GetFinalisedMessage(label, message, color, FeatureInfoContainer.colorEntireMessage);
                Debug.LogError($"{HelperUtils.GetCallerContext(file, line, member)} {finalisedMessage}", context);
            }
        }
        #endregion

        #region Assertion Logs
        public static void LogAssertion(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Assert))
            {
                Debug.LogAssertion($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, FeatureInfoContainer.colorEntireMessage)}");
            }
        }

        public static void LogAssertion(string label, string message, GameObject context)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Assert))
            {
                Debug.LogAssertion($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, FeatureInfoContainer.colorEntireMessage)}", context);
            }
        }
        #endregion

        public static string GetRawFinalisedMessage(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo))
            {
                return $"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, FeatureInfoContainer.colorEntireMessage)}";
            }

            return string.Empty;
        }
    }
}