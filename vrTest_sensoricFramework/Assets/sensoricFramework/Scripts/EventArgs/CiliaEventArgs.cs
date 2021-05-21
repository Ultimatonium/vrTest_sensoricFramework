namespace SensoricFramework
{
    /// <summary>
    /// Derived from <see cref="PlayOlfactoryEventArgs"/> with additional variables to set the light on an Cilia
    /// </summary>
    public class CiliaEventArgs : PlayOlfactoryEventArgs
    {
        /// <summary>
        /// Color which for each Cilia slot
        /// </summary>
        public Neopixel[] light = new Neopixel[6];
        /// <summary>
        /// defines if the color from <see cref="light"/> has to be applied
        /// </summary>
        public bool[] setLight = new bool[6];
    }
}