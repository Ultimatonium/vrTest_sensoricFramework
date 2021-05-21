using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// <c>[Serializable]</c>
    /// Holds all tactile information
    /// </summary>
    [System.Serializable]
    public struct ThermalStruct
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// defines the thermal state
        /// </summary>
        [SerializeField]
        public ThermalEnum thermal;
    }
}