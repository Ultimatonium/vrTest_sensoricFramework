using System;

namespace SensoricFramework
{
    /// <summary>
    /// Holds all information which is needed to emitt an sense
    /// </summary>
    public class SensoricEventArgs : EventArgs
    {
        /// <summary>
        /// holds body part position
        /// </summary>
        public Position position;
        /// <summary>
        /// holds generic sensoric information
        /// </summary>
        public SensoricData sensoric;
    }
}