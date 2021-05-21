using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// <c>[Serializable]</c>
    /// Holds all tactile information
    /// </summary>
    [System.Serializable]
    public struct TactileStruct
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// defines the tactile positions on the hardware normalized between (0,0) and (1,1)
        /// </summary>
        [SerializeField]
        public Vector2[] positions;
    }
}
