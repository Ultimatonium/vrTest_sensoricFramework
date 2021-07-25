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
        public Position position;
        /// <summary>
        /// <c>[SerializeField]</c>
        /// sensoric type: <see cref="SensoricType"/>
        /// </summary>
        [SerializeField]
        public SensoricType[] sensorics = new SensoricType[] { SensoricType.tactile, SensoricType.thermal, SensoricType.olfactory };

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
