using UnityEngine;

namespace FeatureLogger.Runtime
{
    internal static class HelperUtils
    {
        internal static string GetFinalisedMessage(string label, string message, Color color)
        {
            return $"{GetHexColorString(color)} [{label}]</color> {message}";
        }

        internal static string GetHexColorString(Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
        }
    }
}