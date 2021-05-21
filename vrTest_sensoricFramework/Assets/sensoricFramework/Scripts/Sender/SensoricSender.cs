using System.Linq;
using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// abstract base class for all sensoric sender
    /// requires Collider
    /// </summary>
    public abstract class SensoricSender : MonoBehaviour
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// struct which holds all general sensoric informations
        /// </summary>
        [SerializeField]
        public SensoricStruct sensoricStruct;

        protected readonly static Vector3 invalidVector3 = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        /// <summary>
        /// Unity-Message
        /// gets called to initialize the sensoric type by calling <see cref="SetSensoricType"/>
        /// </summary>
        private void Awake()
        {
            sensoricStruct.id = System.Guid.NewGuid().ToString();
            sensoricStruct.sensoric = SetSensoricType();
        }

        /// <summary>
        /// Unity-Message
        /// calls <see cref="CollisionHandler"/> after collision got triggered if <see cref="ExecutionAmountEnum.Once"/>
        /// </summary>
        /// <param name="collision"><see cref="Collision"/></param>
        private void OnCollisionEnter(Collision collision)
        {
            if (sensoricStruct.executionAmount != ExecutionAmountEnum.Once) return;
            CollisionHandler(collision.gameObject, collision.GetContact(0).point, collision.collider);
        }

        /// <summary>
        /// Unity-Message
        /// calls <see cref="CollisionHandler"/> after collision got triggered if <see cref="ExecutionAmountEnum.Ongoing"/>
        /// </summary>
        /// <param name="collision"><see cref="Collision"/></param>
        private void OnCollisionStay(Collision collision)
        {
            if (sensoricStruct.executionAmount != ExecutionAmountEnum.Ongoing) return;
            CollisionHandler(collision.gameObject, collision.GetContact(0).point, collision.collider);
        }

        /// <summary>
        /// Unity-Message
        /// calls <see cref="CollisionHandler"/> after collision got triggered if <see cref="ExecutionAmountEnum.Ongoing"/> with intensity=0 
        /// </summary>
        /// <param name="collision"><see cref="Collision"/></param>
        private void OnCollisionExit(Collision collision)
        {
            if (sensoricStruct.executionAmount != ExecutionAmountEnum.Ongoing) return;
            float intensityBackup = sensoricStruct.intensity;
            sensoricStruct.intensity = 0;
            CollisionHandler(collision.gameObject, invalidVector3, collision.collider);
            sensoricStruct.intensity = intensityBackup;
        }

        /// <summary>
        /// calls <see cref="CollisionHandler"/> after trigger got triggered if <see cref="ExecutionAmountEnum.Once"/>.
        /// determins the point where the trigger got hit with an raycast
        /// </summary>
        /// <param name="other"><see cref="Collider"/></param>
        private void OnTriggerEnter(Collider other)
        {
            if (sensoricStruct.executionAmount != ExecutionAmountEnum.Once) return;
            Rigidbody rigidbody = other.gameObject.GetComponentInParent<Rigidbody>();
            if (rigidbody != null)
            {
                CollisionHandler(rigidbody.gameObject, GetCollisionPointByRaycast(other), other);
            }
        }

        /// <summary>
        /// calls <see cref="CollisionHandler"/> after trigger got triggered if <see cref="ExecutionAmountEnum.Ongoing"/>.
        /// determins the point where the trigger got hit with an raycast
        /// </summary>
        /// <param name="other"><see cref="Collider"/></param>
        private void OnTriggerStay(Collider other)
        {
            if (sensoricStruct.executionAmount != ExecutionAmountEnum.Ongoing) return;
            Rigidbody rigidbody = other.gameObject.GetComponentInParent<Rigidbody>();
            if (rigidbody != null)
            {
                CollisionHandler(rigidbody.gameObject, GetCollisionPointByRaycast(other), other);
            }
        }

        /// <summary>
        /// calls <see cref="CollisionHandler"/> after trigger got triggered if <see cref="ExecutionAmountEnum.Ongoing"/> with intensity=0.
        /// determins the point where the trigger got hit with an raycast
        /// </summary>
        /// <param name="other"><see cref="Collider"/></param>
        private void OnTriggerExit(Collider other)
        {
            if (sensoricStruct.executionAmount != ExecutionAmountEnum.Ongoing) return;
            float intensityBackup = sensoricStruct.intensity;
            sensoricStruct.intensity = 0;
            Rigidbody rigidbody = other.gameObject.GetComponentInParent<Rigidbody>();
            if (rigidbody != null)
            {
                CollisionHandler(rigidbody.gameObject, GetCollisionPointByRaycast(other), other);
            }
            sensoricStruct.intensity = intensityBackup;
        }

        /// <summary>
        /// Unity-Message
        /// Verifies that on the GameObject or on it's childs an Collider exists
        /// </summary>
        private void OnValidate()
        {
            Collider collider = GetComponentInChildren<Collider>();
            if (collider == null)
            {
                Debug.LogWarning("missing collider");
            }
        }

        /// <summary>
        /// if this sender is alowed to emitt an event the <see cref="Play"/> is called.
        /// <see cref="SensoricSenderModifier"/> got applied if there are any
        /// </summary>
        /// <param name="gameObject"><see cref="GameObject"/> of the other collider</param>
        /// <param name="collisionPoint"><see cref="Vector3"/> worldspace position where the Collider got hit</param>
        protected void CollisionHandler(GameObject gameObject, Vector3 collisionPoint, Collider other)
        {
            SensoricReceiver[] sensoricReceivers = gameObject.GetComponentsInChildren<SensoricReceiver>();
            SensoricSenderModifier[] sensoricSenderModifier = GetComponents<SensoricSenderModifier>();
            for (int i = 0; i < sensoricReceivers.Length; i++)
            {
                if (sensoricReceivers[i].sensorics.Contains(sensoricStruct.sensoric))
                {
                    for (int ii = 0; ii < sensoricSenderModifier.Length; ii++)
                    {
                        sensoricSenderModifier[ii]?.Modify(this, sensoricReceivers[i]);
                    }
                    Play(sensoricReceivers[i].position, collisionPoint, other);
                    for (int ii = 0; ii < sensoricSenderModifier.Length; ii++)
                    {
                        sensoricSenderModifier[ii]?.Reset(this, sensoricReceivers[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Creates based of <see cref="SensoricEventArgs"/> for <see cref="SensoricManager"/>
        /// </summary>
        /// <param name="position">defines which body party got hit</param>
        /// <param name="collisionPoint"><see cref="Vector3"/> worldspace position where the Collider got hit</param>
        protected abstract void Play(PositionEnum position, Vector3 collisionPoint, Collider other);

        /// <summary>
        /// has to be implemented to set the sensoric type of this sender
        /// </summary>
        /// <returns><see cref="SensoricEnum"/></returns>
        protected abstract SensoricEnum SetSensoricType();

        /// <summary>
        /// determins the point where the trigger got hit with an raycast
        /// </summary>
        /// <param name="other"><see cref="Collider"/> of the 'other' GameObject</param>
        /// <returns>the hit point when it. Vector3.zero if not</returns>
        private Vector3 GetCollisionPointByRaycast(Collider other)
        {
            RaycastHit hit;
            Collider currentCollider = GetComponent<Collider>();
            Vector3 direction = transform.position - other.gameObject.transform.position;
            Ray ray = new Ray(other.gameObject.transform.position, direction);
            if (currentCollider.Raycast(ray, out hit, direction.sqrMagnitude))
            {
                return hit.point;
            }
            else
            {
                return invalidVector3;
            }
        }
    }
}
