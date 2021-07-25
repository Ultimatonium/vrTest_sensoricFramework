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
        public TactileData tactileStruct;
        /// <summary>
        /// <c>[SerializeField]</c>
        /// bool which tells if an collision point as to be added to the <see cref="tactileStruct"/>
        /// </summary>
        [SerializeField]
        public bool replaceWithCollisionPoint;

        /// <summary>
        /// Creates <see cref="PlayTactileEventArgs"/> for <see cref="SensoricManager"/>
        /// </summary>
        /// <param name="position">defines which body party got hit</param>
        /// <param name="collisionPoint"><see cref="Vector3"/>worldspace position where the Collider got hit</param>
        /// <param name="other">the collider of the other gameObject</param>
        protected override void Play(Position position, Vector3 collisionPoint, Collider other)
        {
            ReplaceWithCollisionPoint(collisionPoint, other);
            SensoricManager.Instance.OnPlayTactile(this, new PlayTactileEventArgs { position = position, sensoric = sensoricStruct, tactile = tactileStruct}); 
        }

        /// <summary>
        /// set type of sensoric
        /// </summary>
        /// <returns><see cref="SensoricType"/></returns>
        protected override SensoricType SetSensoricType()
        {
            return SensoricType.tactile;
        }

        /// <summary>
        /// add the collisionPoint to the <see cref="tactileStruct"/>.
        /// </summary>
        /// <param name="collisionPoint"><see cref="Vector3"/> worldspace position where the Collider got hit</param>
        /// <param name="other">the collider of the other gameObject</param>
        protected void ReplaceWithCollisionPoint(Vector3 collisionPoint, Collider other)
        {
            if (!replaceWithCollisionPoint) return;
            if (collisionPoint == invalidVector3) return;
            if (other is BoxCollider boxCollider)
            {
                tactileStruct.positions.Clear();
                tactileStruct.positions.Add(GetNormalizedCollisionPoint(collisionPoint, boxCollider));
            }
            else
            {
                Debug.LogWarning("addCollisionPoint only works when receiver has a BoxCollider");
            }
        }

        /// <summary>
        /// returns the normalized collision point based on the collidersize
        /// </summary>
        /// <param name="collisionPoint">worldspace position where the Collider got hit</param>
        /// <param name="other">the collider of the other gameObject</param>
        /// <param name="boxCollider">the boxCollider of the other gameObject</param>
        /// <returns>normalized Vector2</returns>
        /// thanks to Andreas P. Arcaro (http://apa-games.com/)
        protected Vector2 GetNormalizedCollisionPoint(Vector3 collisionPoint, BoxCollider boxCollider)
        {
            collisionPoint -= boxCollider.transform.position; //convert from world to local
            Vector3 topLeft    = new Vector3(boxCollider.size.x / 2 * boxCollider.transform.localScale.x * 1
                                            ,boxCollider.size.y / 2 * boxCollider.transform.localScale.y * 1
                                            ,boxCollider.size.z / 2 * boxCollider.transform.localScale.z * 1
                                            );
            Vector3 topRight   = new Vector3(boxCollider.size.x / 2 * boxCollider.transform.localScale.x * -1
                                            ,boxCollider.size.y / 2 * boxCollider.transform.localScale.y * 1
                                            ,boxCollider.size.z / 2 * boxCollider.transform.localScale.z * 1
                                            );
            Vector3 bottomLeft = new Vector3(boxCollider.size.x / 2 * boxCollider.transform.localScale.x * 1
                                            ,boxCollider.size.y / 2 * boxCollider.transform.localScale.y * -1
                                            ,boxCollider.size.z / 2 * boxCollider.transform.localScale.z * 1
                                            );

            Quaternion quaternion = Quaternion.LookRotation(boxCollider.gameObject.transform.forward, boxCollider.gameObject.transform.up);
            topLeft = quaternion * topLeft;
            topRight = quaternion * topRight;
            bottomLeft = quaternion * bottomLeft;
            Vector3 vectorHorizontal = topRight - topLeft;
            Vector3 vectorVertical = bottomLeft - topLeft;
            Vector2 collisionPointNormalized = new Vector2(Vector3.Dot(vectorHorizontal.normalized, (collisionPoint - topLeft) / (boxCollider.transform.localScale.x * boxCollider.size.x))
                                                          ,Vector3.Dot(vectorVertical.normalized, (collisionPoint - topLeft) / (boxCollider.transform.localScale.y * boxCollider.size.y))
                                                          );
            return collisionPointNormalized;
        }
    }
}
