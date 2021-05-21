namespace SensoricFramework
{
    /// <summary>
    /// Holds all information which is needed to emitt an tactile sense
    /// </summary>
    public class PlayTactileEventArgs : SensoricEventArgs
    {
        /// <summary>
        /// struct which holds all olfactory information
        /// </summary>
        public TactileStruct tactile;
    }
}
