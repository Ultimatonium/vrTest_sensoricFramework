using Bhaptics.Tact.Unity;
using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// when collider got hit sends <see cref="BHapticsEventArgs"/> to <see cref="SensoricManager"/>
    /// </summary>
    public class BHapticsSender : TactileSender
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// Holds a clip created by bHaptics Designer
        /// </summary>
        [SerializeField]
        public FileHapticClip bHapticClip;

        /// <summary>
        /// Creates <see cref="BHapticsEventArgs"/> for <see cref="SensoricManager"/>
        /// </summary>
        /// <param name="position">defines which body party got hit</param>
        /// <param name="collisionPoint"><see cref="Vector3"/>worldspace position where the Collider got hit</param>
        /// <param name="other"><see cref="TactileSender"/>t</param>
        protected override void Play(Position position, Vector3 collisionPoint, Collider other)
        {
            ReplaceWithCollisionPoint(collisionPoint, other);
            SensoricManager.Instance.OnPlayTactile(this, new BHapticsEventArgs { position = position, sensoric = sensoricStruct, tactile = tactileStruct, hapticClip = bHapticClip });
        }
    }
}
