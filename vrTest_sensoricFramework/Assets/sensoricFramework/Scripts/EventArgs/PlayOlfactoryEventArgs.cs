namespace SensoricFramework
{
    /// <summary>
    /// Holds all information which is needed to emitt an olfactory sense
    /// </summary>
    public class PlayOlfactoryEventArgs : SensoricEventArgs
    {
        /// <summary>
        /// struct which holds all olfactory information
        /// </summary>
        public OlfactoryData olfactory;
    }
}
