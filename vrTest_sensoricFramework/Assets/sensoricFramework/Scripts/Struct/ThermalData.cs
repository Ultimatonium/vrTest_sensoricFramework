using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// <c>[Serializable]</c>
    /// Holds all tactile information
    /// </summary>
    [System.Serializable]
    public struct ThermalData
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// defines the thermal state
        /// </summary>
        [SerializeField]
        public Thermal thermal;
    }
}