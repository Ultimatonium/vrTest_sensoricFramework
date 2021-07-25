using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// <c>[Serializable]</c>
    /// Holds all olfactory information
    /// </summary>
    [System.Serializable]
    public struct OlfactoryData
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// defines the smell
        /// </summary>
        [SerializeField]
        public Smell smell;
    }
}
