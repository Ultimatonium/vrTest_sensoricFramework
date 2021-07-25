using System;

namespace SensoricFramework
{
    /// <summary>
    /// Holds all information which is needed to emitt an thermal sense
    /// </summary>
    public class PlayThermalEventArgs : SensoricEventArgs
    {
        /// <summary>
        /// struct which holds all thermal information
        /// </summary>
        public ThermalData thermal;
    }
}
