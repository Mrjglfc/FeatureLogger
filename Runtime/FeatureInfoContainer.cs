using UnityEngine;

namespace FeatureLogger.Runtime
{
    [CreateAssetMenu(menuName = "FeatureLogger/FeatureContainer", fileName = "FeatureContainer")]
    public class FeatureInfoContainer : ScriptableObject
    {
        [Header("Global Settings")]
        [Tooltip("Will allow for custom color styling for Warning & Error logs.")]
        public bool overrideConsoleStyling = false;

        [Tooltip("Will use the specified color to style the entire message contents instead of use the group tag")]
        public bool willColorEntireMessageContents = false;

        [Header("Feature-Specific Settings")]
        public FeatureInfo[] featureGroups;
    }
}

