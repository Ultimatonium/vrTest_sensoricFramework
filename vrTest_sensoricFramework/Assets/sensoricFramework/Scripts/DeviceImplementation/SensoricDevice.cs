using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// Abstract class from which every plugin implementation has to be derived from
    /// </summary>
    public abstract class SensoricDevice : MonoBehaviour
    {
        /// <summary>
        /// singelton instance which may be used in future use
        /// </summary>
        private SensoricDevice instance;

        /// <summary>
        /// Unity-Message
        /// in this context: verifies that this script only exists once
        /// </summary>
        private void Awake()
        {
            if (instance == this)
            {
                return;
            }
            if (instance != null)
            {
                Debug.LogWarning("Multiple " + GetType().Name);
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Unity-Message
        /// in this context: initialize the derived plugin. 
        /// If the hardware is connected the plugin will be attached to the manager. 
        /// If not then the plugin will be terminated
        /// </summary>
        private void Start()
        {
            Initialize();
            if (IsConnected())
            {
                AttachToManager();
            }
            else
            {
                Terminate();
            }
        }

        /// <summary>
        /// must contain all logic to setup and startup a plugin and hardware connection
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// verifies if any hardware is connected for this plugin
        /// </summary>
        /// <returns>true if connected hardware is found. false if not</returns>
        protected abstract bool IsConnected();

        /// <summary>
        /// Attaches this plugin to the <see cref="SensoricManager"/>. 
        /// Usually by attaching one or more methods to the events <see cref="PlayTactile"/>, <see cref="PlayThermal"/> or <see cref="PlayOlfactory"/>
        /// </summary>
        protected abstract void AttachToManager();

        /// <summary>
        /// must contain all logic to shutdown the plugin and belonging hardware
        /// </summary>
        protected abstract void Terminate();
    }
}