using UnityEditor;

namespace SensoricFramework
{
    /// <summary>
    /// Contains methods to interact with the <see href="https://docs.unity3d.com/2020.2/Documentation/Manual/class-TagManager.html">TagManager</see>
    /// Uses <see cref="UnityEditor"/> functionality
    /// </summary>
    public static class TagManager
    {
        /// <summary>
        /// <c>[InitializeOnLoadMethod]</c>
        /// Hack to call this "Editor" script by an runtime script. e.g. for OnValidate.
        /// Must be enhanced if needed.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Init()
        {
            CiliaDevice.OnTagExist = TagExist;
        }

        /// <summary>
        /// Verifies if a tag exists in the TagManager
        /// </summary>
        /// <param name="tag">The name of the tag that has to be verified</param>
        /// <returns>true if exists. false if not</returns>
        public static bool TagExist(string tag)
        {
            bool tagFound = false;
            UnityEngine.Object tagManager = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
            SerializedObject serializedTagManager = new SerializedObject(tagManager);
            SerializedProperty serializedProperty = serializedTagManager.FindProperty("tags");
            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                if (serializedProperty.GetArrayElementAtIndex(i).stringValue == tag)
                {
                    tagFound = true;
                    break;
                }
            }
            return tagFound;
        }
    }
}