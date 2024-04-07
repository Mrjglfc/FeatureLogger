using UnityEngine;

namespace FeatureLogger.Runtime
{
    [CreateAssetMenu(menuName = "FeatureLogger/FeatureContainer", fileName = "FeatureContainer")]
    public class FeatureInfoContainer : ScriptableObject
    {
        public FeatureInfo[] featureInfos;
    }
}

