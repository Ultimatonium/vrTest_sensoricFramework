using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// can be attached to any GameObject which holds an SensoricSender. Will modify the values before calling the event and resets it back.
    /// </summary>
    public abstract class SensoricSenderModifier : MonoBehaviour
    {
        /// <summary>
        /// Modifies values
        /// </summary>
        /// <param name="sensoricSender"><see cref="SensoricSender"/></param>
        /// <param name="sensoricReceiver"><see cref="SensoricReceiver"/></param>
        public abstract void Modify(SensoricSender sensoricSender, SensoricReceiver sensoricReceiver = null);

        /// <summary>
        /// resets values
        /// </summary>
        /// <param name="sensoricSender"><see cref="SensoricSender"/></param>
        /// <param name="sensoricReceiver"><see cref="SensoricReceiver"/></param>
        public abstract void Reset(SensoricSender sensoricSender, SensoricReceiver sensoricReceiver = null);
    }
}