using UnityEngine;

namespace FeatureLogger.Runtime
{
    internal static class HelperUtils
    {
        internal const string FEATURELOGGER_TAG = "[FeatureLogger]";

        internal static string GetFinalisedMessage(string label, string message, Color color, bool isMessageContentsStyled)
        {
            if (isMessageContentsStyled)
            {
                return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}> [{label}] {message}</color>";
            }

            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}> [{label}]</color> {message}";
        }

        internal static string GetFinalisedMessageWithoutColour(string label, string message)
        {
            return $"[{label}] {message}";
        }
    }
}