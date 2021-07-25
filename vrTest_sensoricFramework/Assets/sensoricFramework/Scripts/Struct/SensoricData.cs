using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// <c>[Serializable]</c>
    /// Holds all general sensoric information
    /// </summary>
    [System.Serializable]
    public struct SensoricData
    {
        /// <summary>
        /// id which is unique per sender.
        /// needed for bHaptics
        /// </summary>
        [HideInInspector]
        public string id;
        /// <summary>
        /// defines the sensoric type
        /// </summary>
        [HideInInspector]
        public SensoricType sensoric;
        /// <summary>
        /// <c>[SerializeField]</c>
        /// defines the intensity. normalized between 0 and 1 (inclusive)
        /// </summary>
        [SerializeField]
        [Range(0f, 1f)]
        public float intensity;
        /// <summary>
        /// <c>[SerializeField]</c>
        /// defines the duration in float seconds
        /// </summary>
        [SerializeField]
        public float duration;
        /// <summary>
        /// <c>[SerializeField]</c>
        /// defines how often executed
        /// </summary>
        [SerializeField]
        public ExecutionAmount executionAmount;
    }
}