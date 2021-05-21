using UnityEditor;
using UnityEngine;

namespace SensoricFramework
{
    /// <summary>
    /// when collider got hit sends <see cref="PlayTactileEventArgs"/> to <see cref="SensoricManager"/>
    /// requires BoxCollider
    /// </summary>
    public class TactileSender : SensoricSender
    {
        /// <summary>
        /// <c>[SerializeField]</c>
        /// struct which holds all tactile information
        /// </summary>
        [SerializeField]
        public TactileStruct tactileStruct;
        /// <summary>
        /// <c>[SerializeField]</c>
        /// bool which tells if an collision point as to be added to the <see cref="tactileStruct"/>
        /// </summary>
        [SerializeField]
        public bool addCollisionPoint;

        /// <summary>
        /// Creates <see cref="PlayTactileEventArgs"/> for <see cref="SensoricManager"/>
        /// </summary>
        /// <param name="position">defines which body party got hit</param>
        /// <param name="collisionPoint"><see cref="Vector3"/> worldspace position where the Collider got hit</param>
        protected override void Play(PositionEnum position, Vector3 collisionPoint, Collider other)
        {
            AddCollisionPoint(collisionPoint, other);
            SensoricManager.Instance.OnPlayTactile(this, new PlayTactileEventArgs { position = position, sensoric = sensoricStruct, tactile = tactileStruct}); 
        }

        /// <summary>
        /// set type of sensoric
        /// </summary>
        /// <returns><see cref="SensoricEnum"/></returns>
        protected override SensoricEnum SetSensoricType()
        {
            return SensoricEnum.tactile;
        }

        /// <summary>
        /// add the collisionPoint to the <see cref="tactileStruct"/>.
        /// </summary>
        /// <param name="collisionPoint"><see cref="Vector3"/> worldspace position where the Collider got hit</param>
        // thanks to Andreas P. Arcaro (http://apa-games.com/)
        protected void AddCollisionPoint(Vector3 collisionPoint, Collider other)
        {
            return; //todo: finisch implementaion
            /*
            if (!addCollisionPoint) return;
            if (collisionPoint == invalidVector3) return;
            if (other is BoxCollider boxCollider)
            {
                Vector3[] vertices = new Vector3[8];
                gizmosCore = other.transform.position;
                Vector3 p = collisionPoint;
                vertices[0] = new Vector3(other.transform.position.x + boxCollider.size.x / 2 * other.transform.localScale.x * -1
                                         ,other.transform.position.y + boxCollider.size.y / 2 * other.transform.localScale.y * -1
                                         ,other.transform.position.z + boxCollider.size.z / 2 * other.transform.localScale.z * 1
                                         );
                vertices[1] = new Vector3(other.transform.position.x + boxCollider.size.x / 2 * other.transform.localScale.x * 1
                                         ,other.transform.position.y + boxCollider.size.y / 2 * other.transform.localScale.y * -1
                                         ,other.transform.position.z + boxCollider.size.z / 2 * other.transform.localScale.z * 1
                                         );
                vertices[2] = new Vector3(other.transform.position.x + boxCollider.size.x / 2 * other.transform.localScale.x * 1
                                         ,other.transform.position.y + boxCollider.size.y / 2 * other.transform.localScale.y * -1
                                         ,other.transform.position.z + boxCollider.size.z / 2 * other.transform.localScale.z * -1
                                         );
                vertices[3] = new Vector3(other.transform.position.x + boxCollider.size.x / 2 * other.transform.localScale.x * -1
                                         ,other.transform.position.y + boxCollider.size.y / 2 * other.transform.localScale.y * -1
                                         ,other.transform.position.z + boxCollider.size.z / 2 * other.transform.localScale.z * -1
                                         );
                vertices[4] = new Vector3(other.transform.position.x + boxCollider.size.x / 2 * other.transform.localScale.x * -1
                                         ,other.transform.position.y + boxCollider.size.y / 2 * other.transform.localScale.y * 1
                                         ,other.transform.position.z + boxCollider.size.z / 2 * other.transform.localScale.z * 1
                                         );
                vertices[5] = new Vector3(other.transform.position.x + boxCollider.size.x / 2 * other.transform.localScale.x * 1
                                         ,other.transform.position.y + boxCollider.size.y / 2 * other.transform.localScale.y * 1
                                         ,other.transform.position.z + boxCollider.size.z / 2 * other.transform.localScale.z * 1
                                         );
                vertices[6] = new Vector3(other.transform.position.x + boxCollider.size.x / 2 * other.transform.localScale.x * 1
                                         ,other.transform.position.y + boxCollider.size.y / 2 * other.transform.localScale.y * 1
                                         ,other.transform.position.z + boxCollider.size.z / 2 * other.transform.localScale.z * -1
                                         );
                vertices[7] = new Vector3(other.transform.position.x + boxCollider.size.x / 2 * other.transform.localScale.x * -1
                                         ,other.transform.position.y + boxCollider.size.y / 2 * other.transform.localScale.y * 1
                                         ,other.transform.position.z + boxCollider.size.z / 2 * other.transform.localScale.z * -1
                                         );
                gizmosVetices = vertices;
                Vector3 u1 = vertices[7] - vertices[4];
                Vector3 u2 = vertices[0] - vertices[4];

                Vector3 v1 = u1.normalized;
                Vector3 v2 = u2.normalized;

                Vector2 point = new Vector2(Vector3.Dot(u1, p - vertices[4]), Vector3.Dot(u2, p - vertices[4]));
                Debug.Log(point);
            }
            else
            {
                Debug.LogWarning("addCollisionPoint only works when receiver has a BoxCollider");
            }
            */
        }

        /*
        private Vector3 gizmosCore;
        private Vector3[] gizmosVetices = new Vector3[8];

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < gizmosVetices.Length; i++)
            {
                Handles.Label(gizmosVetices[i], "p"+i);
                Gizmos.DrawLine(gizmosCore, gizmosVetices[i]);
            }
        }
        */
    }
}
