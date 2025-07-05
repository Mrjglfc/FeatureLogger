using System.Collections.Generic;
using System.IO;
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
            if (featureInfoContainer == null)
            {
                Debug.LogError($"[FeatureLogger] FeatureInfoContainer is null! Please fill in the inspector reference within the FeatureContainer ScriptableObject");
                return;
            }

            FeatureInfoContainer = featureInfoContainer;

            // Convert to dictionary as logger runtime will need to do fast lookup.
            // Also only pulls values which have been marked as "Enabled".
            features = featureInfoContainer.featureGroups.Where(x => x.Enabled).ToDictionary(x => x.Name, x => x);

            if (features.Count == 0 || features == null)
            {
                Debug.LogWarning($"[FeatureLogger] There are no groups enabled or assigned within the FeatureContainer ScriptableObject.\n" +
                    $"FeatureLogger will not be working as expected");
            }
        }

        #region Debug Logs
        public static void Log(string label, string message,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Debug))
            {
                string header = GetCallerContext(file, line, member);
                string finalMessage = GetFinalisedMessage(label, message, featureInfo.Color);
                Debug.Log($"{header} {finalMessage}");
            }
        }

        public static void Log(string label, string message, GameObject context,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Debug))
            {
                string header = GetCallerContext(file, line, member);
                string finalMessage = GetFinalisedMessage(label, message, featureInfo.Color);
                Debug.Log($"{header} {finalMessage}", context);
            }
        }
        #endregion

        #region Warning Logs
        public static void LogWarning(string label, string message,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Warning))
            {
                Color color = GetMessageColor(featureInfo.Color, Color.yellow);
                string header = GetCallerContext(file, line, member);
                string finalisedMessage = GetFinalisedMessage(label, message, color);
                Debug.LogWarning($"{header} {finalisedMessage}");
            }
        }

        public static void LogWarning(string label, string message, GameObject context,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Warning))
            {
                Color color = GetMessageColor(featureInfo.Color, Color.yellow);
                string header = GetCallerContext(file, line, member);
                string finalisedMessage = GetFinalisedMessage(label, message, color);
                Debug.LogWarning($"{header} {finalisedMessage}", context);
            }
        }
        #endregion

        #region Error Logs
        public static void LogError(string label, string message,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Error))
            {
                Color color = GetMessageColor(featureInfo.Color, Color.red);
                string header = GetCallerContext(file, line, member);
                string finalisedMessage = GetFinalisedMessage(label, message, color); 
                Debug.LogError($"{header} {finalisedMessage}");
            }
        }

        public static void LogError(string label, string message, GameObject context,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Error))
            {
                Color color = GetMessageColor(featureInfo.Color, Color.red);
                string header = GetCallerContext(file, line, member);
                string finalisedMessage = GetFinalisedMessage(label, message, color); 
                Debug.LogError($"{header} {finalisedMessage}", context);
            }
        }
        #endregion

        #region Assertion Logs
        public static void LogAssertion(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Assert))
            {
                Debug.LogAssertion($"{GetFinalisedMessage(label, message, featureInfo.Color)}");
            }
        }

        public static void LogAssertion(string label, string message, GameObject context)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Assert))
            {
                Debug.LogAssertion($"{GetFinalisedMessage(label, message, featureInfo.Color)}", context);
            }
        }
        #endregion

        public string GetRawFinalisedMessage(string label, string message)
        {
            return features.TryGetValue(label, out FeatureInfo featureInfo)
                ? $"{GetFinalisedMessage(label, message, featureInfo.Color)}"
                : string.Empty;
        }

        internal static string GetFinalisedMessage(string label, string message, Color color)
        {
            string colorHexString = ColorUtility.ToHtmlStringRGB(color);
            return FeatureInfoContainer.colorEntireMessage
                ? $"<color=#{colorHexString}> [{label}] {message}</color>"
                : $"<color=#{colorHexString}> [{label}]</color> {message}";
        }

        private static string GetCallerContext(string file, int line, string member)
        {
            return $"[{Path.GetFileName(file)}:{line} - {member}]";
        }

        private static Color GetMessageColor(Color featureColor, Color defaultColorColor)
        {
            return FeatureInfoContainer.overrideConsoleStyling ? featureColor : defaultColorColor;
        }
    }
}