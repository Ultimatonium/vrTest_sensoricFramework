using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// when collider got hit sends <see cref="CiliaEventArgs"/> to <see cref="SensoricManager"/>
    /// </summary>
    public class CiliaSender : OlfactorySender
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// defines what the cilia light color should be set to
        /// </summary>
        [SerializeField]
        public Neopixel[] light = new Neopixel[CiliaDevice.ciliaSlots];
        /// <summary>
        /// <c>[SerializeField]</c>
        /// defines if <see cref="light"/> has to be applied
        /// </summary>
        [SerializeField]
        public bool[] setLight = new bool[CiliaDevice.ciliaSlots];

        /// <summary>
        /// Validates if <see cref="light"/> and <see cref="setLight"/> still has the size of <see cref="ciliaSlots"/> as it's an <c>[SerializeField]</c> and could be changed in inspector
        /// </summary>
        private void OnValidate()
        {
            if (light.Length != CiliaDevice.ciliaSlots)
            {
                Debug.LogError("amount of light has to be " + CiliaDevice.ciliaSlots);
            }
            if (setLight.Length != CiliaDevice.ciliaSlots)
            {
                Debug.LogError("amount of setLight has to be " + CiliaDevice.ciliaSlots);
            }
        }

        /// <summary>
        /// Creates <see cref="CiliaEventArgs"/> for <see cref="SensoricManager"/>
        /// </summary>
        /// <param name="position">defines which body party got hit</param>
        /// <param name="collisionPoint"><see cref="Vector3"/>not used</param>
        protected override void Play(PositionEnum position, Vector3 collisionPoint, Collider other)
        {
            SensoricManager.Instance.OnPlayOlfactory(this, new CiliaEventArgs { position = position, sensoric = sensoricStruct, olfactory = olfactoryStruct, light = light, setLight = setLight });
        }

        
    }
}