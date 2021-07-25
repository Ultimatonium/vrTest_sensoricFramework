using Bhaptics.Tact;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// Holds the implementation for setting up the bHaptic Plugin and usage.
    /// Is used for bHaptic and ThermoReal devices
    /// </summary>
    public class BHapticsDevice : SensoricDevice
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// Holds an BhapticsConfig file which is needed for the <see cref="Initialize"/>
        /// Default Config can be in the bHaptics Plugin (Bhaptics/SDK/Resources/BhapticsConfig)
        /// </summary>
        [SerializeField]
        private BhapticsConfig config;

        /// <summary>
        /// pre allocated <see cref="DotPoint"/> list for bHaptic Gloves. Used for atm for ThermoReal Gloves only
        /// </summary>
        private List<DotPoint> glovePoint = new List<DotPoint> { new DotPoint(0, 0), new DotPoint(1, 0), new DotPoint(2, 0) };

        /// <summary>
        /// Creates a new GameObject with a <see cref="Bhaptics_Setup"/> script and initializes it
        /// </summary>
        protected override void Initialize()
        {
            if (FindObjectOfType<Bhaptics_Setup>() == null)
            {
                GameObject bHapticsSetupObject = new GameObject("Bhaptics_Setup");
                bHapticsSetupObject.SetActive(false); //to set config before awake
                Bhaptics_Setup bhapticsSetup = bHapticsSetupObject.AddComponent<Bhaptics_Setup>();
                bhapticsSetup.Config = config;
                bHapticsSetupObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Bhaptics_Setup already created");
            }
        }

        /// <summary>
        /// verifies if any bHaptics device is connected
        /// </summary>
        /// <returns>true if any is connected. false if not</returns>
        protected override bool IsConnected()
        {
            bool isConnected = false;
            foreach (PositionType position in Enum.GetValues(typeof(PositionType)))
            {
                isConnected = BhapticsManager.GetHaptic().IsConnect(position);
                if (isConnected) break;
            }
            return isConnected;
        }

        /// <summary>
        /// Attaches events for palying sensoric feedback to the <see cref="SensoricManager"/>
        /// <seealso cref="PlayTactile(object, PlayTactileEventArgs)"/>
        /// <seealso cref="PlayThermal(object, PlayThermalEventArgs)"/>
        /// </summary>
        protected override void AttachToManager()
        {
            SensoricManager.Instance.PlayTactile += PlayTactile;
            SensoricManager.Instance.PlayThermal += PlayThermal;
        }

        /// <summary>
        /// Shuts down the bHaptic pluging and destroies the GameObject
        /// </summary>
        protected override void Terminate()
        {
            BhapticsManager.Dispose();
            Bhaptics_Setup[] bhaptics_Setups = FindObjectsOfType<Bhaptics_Setup>();
            for (int i = 0; i < bhaptics_Setups.Length; i++)
            {
                Destroy(bhaptics_Setups[i].gameObject);
            }

        }

        /// <summary>
        /// Sets up data and sends it to the bHaptic service which sends it to the hardware
        /// Implemented Gloves only
        /// </summary>
        /// <param name="sender">object which calls the event. Usually from a <see cref="SensoricSender"/></param>
        /// <param name="e">Holds any <see cref="EventArgs"/> derived from <see cref="PlayThermalEventArgs"/></param>
        private void PlayThermal(object sender, PlayThermalEventArgs e)
        {
            int dotIntensity = (int)(e.sensoric.intensity * 100 / 2);
            if (e.thermal.thermal == Thermal.hot)
            {
                dotIntensity += 50;
            }
            int durationMillis = (int)(e.sensoric.duration * 1000);
            e.sensoric.id += e.position; //fix for possible bHaptics bug
            switch (e.position)
            {
                case Position.LeftHand:
                    SetGloveIntensity(dotIntensity);
                    BhapticsManager.GetHaptic().Submit(e.sensoric.id, PositionType.HandL, glovePoint, durationMillis);
                    break;
                case Position.RightHand:
                    SetGloveIntensity(dotIntensity);
                    BhapticsManager.GetHaptic().Submit(e.sensoric.id, PositionType.HandR, glovePoint, durationMillis);
                    break;
                default:
                    Debug.LogWarning(e.position + " is not implemented");
                    break;
            }
        }

        /// <summary>
        /// Sets up data and sends it to the bHaptic service which sends it to the hardware
        /// If <paramref name="e"/> is a specification <see cref="BHapticsEventArgs"/> then the accoring bHaptics Clip will be played instead.
        /// </summary>
        /// <param name="sender">object which calls the event. Usually from a <see cref="SensoricSender"/></param>
        /// <param name="e">Holds any <see cref="EventArgs"/> derived from <see cref="PlayTactileEventArgs"/></param>
        private void PlayTactile(object sender, PlayTactileEventArgs e)
        {
            if (e is BHapticsEventArgs bHapticsE)
            {
                if (!BhapticsManager.GetHaptic().IsFeedbackRegistered(bHapticsE.hapticClip.GetAssetID()))
                {
                    BhapticsManager.GetHaptic().RegisterTactFileStr(bHapticsE.hapticClip.GetAssetID(), bHapticsE.hapticClip.JsonValue);
                }
                BhapticsManager.GetHaptic().SubmitRegistered(bHapticsE.hapticClip.GetAssetID());
            }
            else
            {
                List<PathPoint> dots = new List<PathPoint>();
                for (int i = 0; i < e.tactile.positions.Count; i++)
                {
                    dots.Add(new PathPoint(e.tactile.positions[i].x, e.tactile.positions[i].y, (int)(e.sensoric.intensity * 100), e.tactile.positions.Count));
                }
                int durationMillis = (int)(e.sensoric.duration * 1000);
                e.sensoric.id += e.position; //fix for possible bHaptics bug
                switch (e.position)
                {
                    case Position.Head:
                        BhapticsManager.GetHaptic().Submit(e.sensoric.id, PositionType.Head, dots, durationMillis);
                        break;
                    case Position.LeftArm:
                        BhapticsManager.GetHaptic().Submit(e.sensoric.id, PositionType.ForearmL, dots, durationMillis);
                        break;
                    case Position.RightArm:
                        BhapticsManager.GetHaptic().Submit(e.sensoric.id, PositionType.ForearmR, dots, durationMillis);
                        break;
                    case Position.LeftHand:
                        BhapticsManager.GetHaptic().Submit(e.sensoric.id, PositionType.HandL, dots, durationMillis);
                        break;
                    case Position.RightHand:
                        BhapticsManager.GetHaptic().Submit(e.sensoric.id.ToString(), PositionType.HandR, dots, durationMillis);
                        break;
                    case Position.LeftFoot:
                        BhapticsManager.GetHaptic().Submit(e.sensoric.id.ToString(), PositionType.FootL, dots, durationMillis);
                        break;
                    case Position.RightFoot:
                        BhapticsManager.GetHaptic().Submit(e.sensoric.id, PositionType.FootR, dots, durationMillis);
                        break;
                    case Position.ChestFront:
                        BhapticsManager.GetHaptic().Submit(e.sensoric.id, PositionType.VestFront, dots, durationMillis);
                        break;
                    case Position.ChestBack:
                        BhapticsManager.GetHaptic().Submit(e.sensoric.id, PositionType.VestBack, dots, durationMillis);
                        break;
                    default:
                        Debug.LogWarning(e.position + " is not implemented");
                        break;
                }
            }

        }

        /// <summary>
        /// internal method for setting up <see cref="glovePoint"/>
        /// </summary>
        /// <param name="intensity">int value betweeen 0 and 100</param>
        private void SetGloveIntensity(int intensity)
        {
            for (int i = 0; i < glovePoint.Count; i++)
            {
                glovePoint[i].Intensity = intensity;
            }
        }
    }
}