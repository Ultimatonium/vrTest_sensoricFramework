using Bhaptics.Tact.Unity;

namespace SensoricFramework
{
    /// <summary>
    /// Derived from <see cref="PlayTactileEventArgs"/> with an additional variable for an bHaptic Clip
    /// </summary>
    public class BHapticsEventArgs : PlayTactileEventArgs
    {
        /// <summary>
        /// bHaptic Clip created with bHaptics Designer
        /// </summary>
        public FileHapticClip hapticClip;
    }
}