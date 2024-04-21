using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FeatureLogger.Runtime
{
    public class FeatureLoggerRuntime : MonoBehaviour
    {
        [SerializeField] private FeatureInfoContainer featureInfoContainer;
        private static Dictionary<string, FeatureInfo> features;

        private static bool overrideConsoleStyling;
        private static bool willColorEntireMessageContents;

        private void Awake()
        {
            // Null & bounds check to make sure container has data inside.
            if (featureInfoContainer == null)
            {
                Debug.LogError($"{HelperUtils.FEATURELOGGER_TAG} FeatureInfoContainer is null! Please fill in the inspector reference within the FeatureContainer ScriptableObject");
                return;
            }

            // Convert to dictionary as logger runtime will need to do fast lookup.
            // Also only pulls values which have been marked as "Enabled".
            features = featureInfoContainer.featureGroups.Where(x => x.Enabled).ToDictionary(x => x.Name, x => x);

            if (features.Count == 0 || features == null)
            {
                Debug.LogWarning($"{HelperUtils.FEATURELOGGER_TAG} There are no groups enabled or assigned within the FeatureContainer ScriptableObject.\n" +
                    $"FeatureLogger will not be working as expected");
            }

            // Assigning static variables in here
            overrideConsoleStyling = featureInfoContainer.overrideConsoleStyling;
            willColorEntireMessageContents = featureInfoContainer.willColorEntireMessageContents;
        }

        #region Debug Logs
        public static void Log(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Debug))
            {
                Debug.Log($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, willColorEntireMessageContents)}");
            }
        }

        public static void Log(string label, string message, GameObject context)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Debug))
            {
                Debug.Log($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, willColorEntireMessageContents)}", context);
            }
        }
        #endregion

        #region Warning Logs
        public static void LogWarning(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Warning))
            {
                if (overrideConsoleStyling)
                {
                    Debug.LogWarning($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, willColorEntireMessageContents)}");
                }
                else
                {
                    Debug.LogWarning($"{HelperUtils.GetFinalisedMessageWithoutColour(label, message)}");
                }
            }
        }

        public static void LogWarning(string label, string message, GameObject context)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Warning))
            {
                if (overrideConsoleStyling)
                {
                    Debug.LogWarning($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, willColorEntireMessageContents)}", context);
                }
                else
                {
                    Debug.LogWarning($"{HelperUtils.GetFinalisedMessageWithoutColour(label, message)}", context);
                }
            }
        }
        #endregion

        #region Error Logs
        public static void LogError(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Error))
            {
                if (overrideConsoleStyling)
                {
                    Debug.LogError($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, willColorEntireMessageContents)}");
                }
                else
                {
                    Debug.LogError($"{HelperUtils.GetFinalisedMessageWithoutColour(label, message)}");
                }
            }
        }

        public static void LogError(string label, string message, GameObject context)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Error))
            {
                if (overrideConsoleStyling)
                {
                    Debug.LogError($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, willColorEntireMessageContents)}", context);
                }
                else
                {
                    Debug.LogError($"{HelperUtils.GetFinalisedMessageWithoutColour(label, message)}", context);
                }
            }
        }
        #endregion

        #region Assertion Logs
        public static void LogAssertion(string label, string message)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Assert))
            {
                Debug.LogAssertion($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, willColorEntireMessageContents)}");
            }
        }

        public static void LogAssertion(string label, string message, GameObject context)
        {
            if (features.TryGetValue(label, out FeatureInfo featureInfo) && featureInfo.Severity.HasFlag(LogSeverityEnum.Assert))
            {
                Debug.LogAssertion($"{HelperUtils.GetFinalisedMessage(label, message, featureInfo.Color, willColorEntireMessageContents)}", context);
            }
        }
        #endregion
    }
}