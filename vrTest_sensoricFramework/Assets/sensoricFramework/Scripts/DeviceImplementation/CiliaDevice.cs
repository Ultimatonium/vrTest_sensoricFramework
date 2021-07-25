using System;
using System.Collections;
using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// Holds the implementation for setting up the cilia Plugin and usage.
    /// todo: implement surround position
    /// </summary>
    public class CiliaDevice : SensoricDevice
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// Sets the default color of the 6 cilia lights
        /// </summary>
        [SerializeField]
        protected Color[] defaultLightColor = new Color[ciliaSlots];

        /// <summary>
        /// holds the instance of an <see cref="Cilia"/> created in <see cref="Initialize"/>
        /// </summary>
        private Cilia cilia;

        /// <summary>
        /// defines the maximum available slots in an physical Cilia
        /// </summary>
        public const int ciliaSlots = 6;

        /// <summary>
        /// Name of the Cilia Tag
        /// </summary>
        private const string ciliaTag = "Cilia";

        /// <summary>
        /// delegate for TagManager.TagExist
        /// </summary>
        /// <seealso cref="TagManager"/>
        public delegate bool TagExist(string tag);

        /// <summary>
        /// get set by TagManager.Init
        /// </summary>
        /// <seealso cref="TagManager"/>
        public static TagExist OnTagExist;

        /// <summary>
        /// Unity-Message
        /// Validates if <see cref="defaultLightColor"/> still has the size of <see cref="ciliaSlots"/> as it's an <c>[SerializeField]</c> and could be changed in inspector.
        /// Validates if the tag Cilia exists.
        /// </summary>
        private void OnValidate()
        {
            //validate light array size
            if (defaultLightColor.Length != ciliaSlots)
            {
                Debug.LogError("defaultLightColor amount not same as ciliaSlots");
            }

            //validate Clilia tag
            if (OnTagExist != null && !OnTagExist(ciliaTag))
            {
                Debug.LogError("missing tag: " + ciliaTag);
            }
        }

        /// <summary>
        /// Creates a new GameObject with a <see cref="Cilia"/> script and initializes it
        /// </summary>
        protected override void Initialize()
        {
            if (FindObjectOfType<Cilia>() == null)
            {
                GameObject ciliaObject = new GameObject("Cilia");
                ciliaObject.SetActive(false); //to set serialized fields before awake
                ciliaObject.tag = "Cilia";
                cilia = ciliaObject.AddComponent<Cilia>();
                cilia.GameProfileName = Application.productName;
                //cilia.DefaultSurroundGroup = tbd
                SetCiliaSmells();
                SetCiliaDefaultLight();
                cilia.PathToCiliaPluginScripts = "Assets/sensoricFramework/Plugins/CiliaPlugin/Scripts/";
                ciliaObject.SetActive(true);
                if (Application.isEditor)
                {
                    cilia.AddSmellsList();
                }
                cilia.Initialize();
            }
            else
            {
                Debug.LogWarning("Cilia already created");
            }
        }

        /// <summary>
        /// returns if a connection is established with the Cilia service
        /// </summary>
        /// <returns>true if a connection exists. false if not</returns>
        protected override bool IsConnected()
        {
            return Cilia.mIsConnected;
        }

        /// <summary>
        /// Attaches event for palying sensoric feedback to the <see cref="SensoricManager"/>
        /// </summary>
        protected override void AttachToManager()
        {
            SensoricManager.Instance.PlayOlfactory += PlayOlfactory;
        }

        /// <summary>
        /// Shuts down the cilia pluging and destroies the GameObject
        /// </summary>
        protected override void Terminate()
        {
            cilia.OnApplicationQuit();
            Cilia[] cilias = FindObjectsOfType<Cilia>();
            for (int i = 0; i < cilias.Length; i++)
            {
                Destroy(cilias[i].gameObject);
            }
        }

        /// <summary>
        /// Set <see cref="defaultLightColor"/> to the <see cref="Cilia"/> plugin
        /// </summary>
        private void SetCiliaDefaultLight()
        {
            cilia.Light1 = ConvertColorToNeopixel(defaultLightColor[0]);
            cilia.Light2 = ConvertColorToNeopixel(defaultLightColor[1]);
            cilia.Light3 = ConvertColorToNeopixel(defaultLightColor[2]);
            cilia.Light4 = ConvertColorToNeopixel(defaultLightColor[3]);
            cilia.Light5 = ConvertColorToNeopixel(defaultLightColor[4]);
            cilia.Light6 = ConvertColorToNeopixel(defaultLightColor[5]);
        }

        /// <summary>
        /// initialize and set smells to the Cilia smell library
        /// </summary>
        private void SetCiliaSmells()
        {
            //init
            for (int i = 0; i < cilia.SmellsToAddToSmellLibrary.Length; i++)
            {
                cilia.SmellsToAddToSmellLibrary[i] = "not_defined_" + i;
            }
            //verify
            if (SensoricManager.Instance.smells.Length > ciliaSlots)
            {
                Debug.LogWarning("more smells configured what Cilia can handle");
            }
            //set
            for (int i = 0; i < SensoricManager.Instance.smells.Length && i < 6; i++)
            {
                cilia.SmellsToAddToSmellLibrary[i] = SensoricManager.Instance.smells[i];
            }
        }

        /// <summary>
        /// converts RGB of <see cref="Color"/> to an <see cref="Neopixel"/>
        /// </summary>
        /// <param name="color"><see cref="Color"/></param>
        /// <returns><see cref="Neopixel"/></returns>
        private Neopixel ConvertColorToNeopixel(Color color)
        {
            return new Neopixel { redValue = (byte)(color.r * byte.MaxValue), greenValue = (byte)(color.g * byte.MaxValue), blueValue = (byte)(color.b * byte.MaxValue) };
        }

        /// <summary>
        /// sends smell with a coroutine for set duration with the plugin to the hardware
        /// if <paramref name="e"/> is of type <see cref="CiliaEventArgs"/> then acoring colors are set as well
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayOlfactory(object sender, PlayOlfactoryEventArgs e)
        {
            if (e is CiliaEventArgs ciliaE)
            {
                for (uint i = 0; i < ciliaE.light.Length; i++)
                {
                    if (ciliaE.setLight[i])
                    {
                        Cilia.setLight(SurroundPosition.All, i + 1, ciliaE.light[i].redValue, ciliaE.light[i].greenValue, ciliaE.light[i].blueValue);
                    }
                }
            }
            StartCoroutine(SetFanWithDuration(e.sensoric.duration, SurroundPosition.All, (SmellList)Enum.Parse(typeof(SmellList), e.olfactory.smell.ToString()), (byte)(e.sensoric.intensity * byte.MaxValue)));
        }

        /// <summary>
        /// Coroutine which sets the fan speed for specified smell to set speed and stops it after set duration
        /// </summary>
        /// <param name="seconds">float duration in seconds</param>
        /// <param name="surroundPosition"><see cref="SurroundPosition"/></param>
        /// <param name="smell"><see cref="SmellList"/></param>
        /// <param name="fanSpeed">fan intensity from 0 to 255</param>
        /// <returns></returns>
        private IEnumerator SetFanWithDuration(float seconds, SurroundPosition surroundPosition, SmellList smell, byte fanSpeed)
        {
            Cilia.setFan(surroundPosition, smell, fanSpeed);
            yield return new WaitForSeconds(seconds);
            Cilia.setFan(surroundPosition, smell, 0);
        }
    }
}