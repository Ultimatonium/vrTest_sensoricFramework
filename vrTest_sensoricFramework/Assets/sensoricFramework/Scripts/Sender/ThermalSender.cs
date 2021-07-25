using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// when collider got hit sends <see cref="PlayThermalEventArgs"/> to <see cref="SensoricManager"/>
    /// </summary>
    public class ThermalSender : SensoricSender
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// struct which holds all thermal information
        /// </summary>
        [SerializeField]
        public ThermalData thermalStruct;

        /// <summary>
        /// Creates <see cref="PlayThermalEventArgs"/> for <see cref="SensoricManager"/>
        /// </summary>
        /// <param name="position">defines which body party got hit</param>
        /// <param name="collisionPoint"><see cref="Vector3"/>not used</param>
        /// <param name="other"><see cref="TactileSender"/>t</param>
        protected override void Play(Position position, Vector3 collisionPoint, Collider other)
        {
            SensoricManager.Instance.OnPlayThermal(this, new PlayThermalEventArgs {position = position, sensoric = sensoricStruct, thermal = thermalStruct });
        }

        /// <summary>
        /// set type of sensoric
        /// </summary>
        /// <returns><see cref="SensoricType"/></returns>
        protected override SensoricType SetSensoricType()
        {
            return SensoricType.thermal;
        }
    }
}
