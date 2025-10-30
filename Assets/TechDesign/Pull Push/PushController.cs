using System;
using UnityEngine;

namespace Player
{
    public class PushController : MonoBehaviour
    {
        public static PushController instance;
        
        
        [Header("Push Settings")] [Tooltip("How far the player can detect pushable objects.")]
        public float detectionRadius = 2f;

        public float detectionDistance = 3f;
        public LayerMask pushableLayer; //What layer of objects can be pushed

        public string pushableTag = "Pushable"; //Tag for what objects can be pushed

        [Header("Push Movement")] public float pushSpeed = 5f;
        public LayerMask collisionLayers; //Layer that box cannot push through

        private GameObject pushedObj;
        private Vector3 targetPosition;
        private bool isPushing;

        //Gizmos
        private Vector3 lastCheckOrigin;
        private Vector3 lastHalfExtents;
        private bool lastPathClear;

        private void Awake()
        {
            instance ??= this;
        }

        void Update()
        {
           // HandleInput();

            if (isPushing && pushedObj != null)
            {
                MovePushedObject();
            }
        }

        // private void HandleInput()
        // {
        //     if (Input.GetKeyDown(interactKey) && !isPushing)
        //     {
        //         TryPush();
        //     }
        // }

        public void TryPush()
        {
           Debug.Log("Trying to push");
            //Finds pushable object
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionRadius, transform.forward,
                detectionDistance, pushableLayer);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag(pushableTag)) //Checks object is pushable
                {
                    StartPush(hit);
                    break;
                }
            }
        }

        private void StartPush(RaycastHit hit)
        {
            pushedObj = hit.collider.gameObject;
            Collider col = pushedObj.GetComponent<Collider>();
            if (col == null) return;

            string side = GetPushSide(hit); //Finds side of object being looked at by calling get push side
            Vector3
                pushDir = GetPushDirection(hit.transform, side); //Converts side of object into world direction vector

            //Finds how far to push object based on size
            float pushDistance = Mathf.Abs(pushDir.x) > Mathf.Abs(pushDir.z) ? col.bounds.size.x : col.bounds.size.z;

            //Calculates what position the object will be pushed to
            Vector3 proposedTarget = pushedObj.transform.position + pushDir * pushDistance;

            //Finds if the space is blocked or if the position can be pushed to
            if (IsPathClear(pushedObj.transform.position, pushDir, pushDistance, col))
            {
                targetPosition = proposedTarget;
                isPushing = true;
            }
            else
            {
                Debug.Log("Position blocked");
            }
        }

        private void MovePushedObject()
        {
            pushedObj.transform.position = Vector3.MoveTowards(pushedObj.transform.position, targetPosition,
                pushSpeed * Time.deltaTime);

            //Checks if object reached target within threshold as to avoid being off by minor amount
            if (Vector3.Distance(pushedObj.transform.position, targetPosition) < 0.01f)
            {
                //Snaps to position to avoid imprecision
                pushedObj.transform.position = new Vector3(Mathf.Round(targetPosition.x * 1000f) / 1000f,
                    Mathf.Round(targetPosition.y * 1000f) / 1000f, Mathf.Round(targetPosition.z * 1000f) / 1000f);

                isPushing = false;
            }
        }

        //Checks for clear path
        private bool IsPathClear(Vector3 startPos, Vector3 pushDir, float distance, Collider col)
        {
            Vector3 checkOrigin = startPos + pushDir * distance; //Checks one object length ahead
            //Shrinks check slightly to give more leeway
            Vector3 halfExtents = new Vector3(col.bounds.extents.x * 0.95f, col.bounds.extents.y,
                col.bounds.extents.z * 0.95f);

            //Checks for colliders in checked space
            Collider[] hits = Physics.OverlapBox(checkOrigin, halfExtents, Quaternion.identity, collisionLayers);

            //Gizmo settings
            lastCheckOrigin = checkOrigin;
            lastHalfExtents = halfExtents;
            lastPathClear = hits.Length == 0;

            return hits.Length == 0; //Returns true if no colliders found
        }

        //Called mid start push script
        private string GetPushSide(RaycastHit hit)
        {
            Vector3 toPlayer =
                (transform.position - hit.transform.position).normalized; //Finds direction from object to player
            Vector3 localDir =
                hit.transform.InverseTransformDirection(toPlayer); //Converts direction to objects local to find side
            localDir.y = 0;

            //Finds which side the player is mostly facing
            return Mathf.Abs(localDir.x) > Mathf.Abs(localDir.z)
                ? (localDir.x > 0 ? "Right" : "Left")
                : (localDir.z > 0 ? "Front" : "Back");
        }

        private Vector3 GetPushDirection(Transform objTransform, string side)
        {
            return side switch
            {
                "Left" => objTransform.right,
                "Right" => -objTransform.right,
                "Front" => -objTransform.forward,
                "Back" => objTransform.forward,
                _ => Vector3.zero
            };
        }

        private void OnDrawGizmos()
        {
            //Draw detection range
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Vector3 castEnd = transform.position + transform.forward * detectionDistance;
            Gizmos.DrawWireSphere(castEnd, detectionRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, castEnd);

            //Draw push path
            if (pushedObj != null && isPushing)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(pushedObj.transform.position, targetPosition);
                Gizmos.DrawWireSphere(targetPosition, 0.1f);
            }

            //Draw overlap box for collision debug
            if (lastCheckOrigin != Vector3.zero)
            {
                Gizmos.color = lastPathClear ? Color.green : Color.red;
                Gizmos.matrix = Matrix4x4.TRS(lastCheckOrigin, Quaternion.identity, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, lastHalfExtents * 2);
            }
        }
    }
}
