using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// defines human body position which can receive an sensoric event
    /// </summary>
    public class SensoricReceiver : MonoBehaviour
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// body part
        /// </summary>
        [SerializeField]
        public PositionEnum position;
        /// <summary>
        /// <c>[SerializeField]</c>
        /// sensoric type: <see cref="SensoricEnum"/>
        /// </summary>
        [SerializeField]
        public SensoricEnum[] sensorics = new SensoricEnum[] { SensoricEnum.tactile, SensoricEnum.thermal, SensoricEnum.olfactory };

        /// <summary>
        /// Unity-Message
        /// Verifies that on the GameObject or on it's childs an Collider exists.
        /// Verifies that on the GameObject or on it's parent an rigidbody exists.
        /// </summary>
        private void OnValidate()
        {
            Collider collider = GetComponentInChildren<Collider>();
            if (collider == null)
            {
                Debug.LogWarning("missing collider");
            }
            Rigidbody rigidbody = GetComponentInParent<Rigidbody>();
            if (rigidbody == null)
            {
                Debug.LogError("missing Rigidbody");
            }
        }
    }
}
