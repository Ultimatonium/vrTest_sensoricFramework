using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// Modifies the intensity based on the distance. the nearer the higher.
    /// </summary>
    public class LinearIncreaseByDistanceModifier : SensoricSenderModifier
    {
        /// <summary>
        /// backup for <see cref="Reset"/>
        /// </summary>
        private float intensityBackup;

        /// <summary>
        /// changes the intensity based on the distance
        /// </summary>
        /// <param name="sensoricSender"><see cref="SensoricSender"/></param>
        /// <param name="sensoricReceiver"><see cref="SensoricReceiver"/></param>
        public override void Modify(SensoricSender sensoricSender, SensoricReceiver sensoricReceiver = null)
        {
            intensityBackup = sensoricSender.sensoricStruct.intensity;
            if (sensoricReceiver == null) return;
            float maxDistanceToCenter = 1;
            float maxScaleFactor = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
            Collider collider = sensoricSender.gameObject.GetComponent<Collider>();
            switch (collider)
            {
                case BoxCollider bc:
                    maxDistanceToCenter = (Mathf.Max(bc.size.x, bc.size.y, bc.size.z) / 2) * maxScaleFactor;
                    break;
                case SphereCollider sc:
                    maxDistanceToCenter = sc.radius * maxScaleFactor;
                    break;
                default:
                    Debug.LogError("unhandeld collider");
                    break;
            }
            float distanceFactor = (1 - (Vector3.Distance(sensoricReceiver.gameObject.transform.position, sensoricSender.gameObject.transform.position) / maxDistanceToCenter));
            sensoricSender.sensoricStruct.intensity *= distanceFactor;
            sensoricSender.sensoricStruct.intensity = Mathf.Clamp01(sensoricSender.sensoricStruct.intensity); //quickfix
        }

        /// <summary>
        /// reset intensity to original Values
        /// </summary>
        /// <param name="sensoricSender"><see cref="SensoricSender"/></param>
        /// <param name="sensoricReceiver"><see cref="SensoricReceiver"/></param>
        public override void Reset(SensoricSender sensoricSender, SensoricReceiver sensoricReceiver = null)
        {
            sensoricSender.sensoricStruct.intensity = intensityBackup;
        }
    }
}