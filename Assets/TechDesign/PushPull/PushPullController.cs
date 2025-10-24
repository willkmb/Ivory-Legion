using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PushPullController : MonoBehaviour
{
    //Values that effect before grabbing an object
    [Header("What Can Be Grabbed")]
    public float sphereRadius = 1f;         //How big the grab radius is
    public float maxDistance = 1f;          //From how far away you can grab from
    public string pushableTag = "Pushable"; //What tag an object needs to be grabbed
    public LayerMask pushableLayer;         //What layer an object needs to be on to be "seen" by the raycast

    //Values that effect while grabbing an object
    [Header("Grabbing")]
    public KeyCode interactKey = KeyCode.E; //What key needs to be pressed to grab
    private bool isGrabbing;                //bool to check if grabbing an object
    public float distanceFromObj = 1.5f;    //What distance the "player" is kept from grabbed object
    private GameObject pushedObj = null;    //Where the gameobject being grabbed is stored

    [Header("Collision Check")]
    public float wallDistance = 1f;         //Value for size of grabbed object so walls cant be walked through
    public LayerMask collisionLayers;       //Layer storage for layers that cant be walked through
    private float halfDepth;                //Depth for grabbed object
    private float halfWidth;                //Width for grabbed object
    private bool whichSide;                 //bool to find what Axis the object is being grabbed from

    private void Start()
    {
        isGrabbing = false;                 //Makes sure that the player hasnt got anything they're grabbing on start
    }

    private void Update()
    {
        GrabCheck();

        //Makes sure somethings being grabbed so collision can be acitvated
        if (isGrabbing && pushedObj != null)
        {
            ActivateCollision();
        }
    }

    //Started from Update
    private void GrabCheck()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("E pressed");

            if (!isGrabbing)    //Checks if nothing is being grabbed currently
            {
                Debug.Log("Not holding");

                //Sends out raycast to find the object that wants to be grabbed
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, transform.forward, maxDistance, pushableLayer);

                foreach (RaycastHit hit in hits)    //Finds the object being hit by raycast
                {
                    if (hit.collider.CompareTag(pushableTag))   //Checks if object has the correct pushable tag
                    {
                        Debug.Log("Ray hit tag");    
                        //Activates GetPlayerViewSide function to find what side is being looked at and assigns that to whatSide
                        string whatSide = GetPlayerViewSide(hit);   
                        Debug.Log("Player is looking at the " + whatSide + " side of the object.");
                        MovePlayer(hit, whatSide);  //Starts MovePlayer function with what object is hit and what side is hit
                    }
                    else
                    {
                        Debug.Log("Nothing to grab");
                    }
                }
            }
            else
            {
                ReleaseObject();    //Releases object if something is being grabbed currently
            }
        }
    }

    private void MovePlayer(RaycastHit hit, string whatSide)    //Moves player to the object and the side that is being interacted with
    {
        Vector3 targetPos = hit.transform.position; //Finds position of moveable object

        if (whatSide == "Left")         //Moves player to "Left" side of object
        {
            targetPos = hit.transform.position - hit.transform.right * distanceFromObj;
            whichSide = true;   //Allows script to know the player is on the X axis
        }
        else if (whatSide == "Right")   //Moves player to "Right" side of object
        {
            targetPos = hit.transform.position + hit.transform.right * distanceFromObj;
            whichSide = true;   //Allows script to know the player is on the X axis
        }
        else if (whatSide == "Front")   //Moves player to "Front" side of object
        {
            targetPos = hit.transform.position + hit.transform.forward * distanceFromObj;
            whichSide = false;  //Allows script to know the player is on the Y axis
        }
        else if (whatSide == "Back")    //Moves player to "Back" side of object
        {
            targetPos = hit.transform.position - hit.transform.forward * distanceFromObj;
            whichSide = false;  //Allows script to know the player is on the Y axis
        }

        targetPos.y = transform.position.y; //Makes sure the Y value isnt changed for the player
        transform.position = targetPos; //Moves the player to target

        PushObject(); //Push Object starts after the player has been moved
    }

    private void PushObject()
    {
        if (pushedObj == null)
        {
            //Finds the object being pushed again
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, transform.forward, maxDistance, pushableLayer);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag(pushableTag))
                {
                    //DISABLE "LEFT" AND "RIGHT" CONTROLS IN THE PLAYER CONTROLLER

                    //VVV Sets the grabbed object as a child of player so they can "push" it about VVV
                    pushedObj = hit.collider.gameObject;

                    pushedObj.transform.SetParent(transform);

                    Collider col = pushedObj.GetComponent<Collider>();

                    if (col != null)
                    {
                        halfWidth = col.bounds.extents.x;
                        halfDepth = col.bounds.extents.z;
                    }

                    Debug.Log("Object grabbed: " + pushedObj.name);
                    isGrabbing = true;
                    break;
                }
            }
        }
    }

    //Releases object if something is being grabbed currently
    private void ReleaseObject()
    {
        if (pushedObj != null)
        {
            //ENABLE "LEFT" AND "RIGHT" CONTROLS IN PLAYER CONTROLLER
            //ENABLE FORWARD CONTROLS IN PLAYER CONTROLLER
            pushedObj.transform.SetParent(null);
            Debug.Log("Object released: " + pushedObj.name);
            pushedObj = null;
            isGrabbing = false;
        }
    }

    private void ActivateCollision()
    {
        Vector3 origin = pushedObj.transform.position;  //Finds the pushed object origin
        Vector3 direction = transform.forward;          //finds the forward direction of the pushed object

        if (whichSide)
        {
            wallDistance = halfWidth;
        }
        else
        {
            wallDistance = halfDepth;
        }

        //Raycast to check if wall is behind the pushed object
        if (Physics.Raycast(origin, direction, out RaycastHit hit, wallDistance + 0.1f, collisionLayers))
        {
            if (hit.collider.gameObject != pushedObj)
            {
                Debug.DrawRay(origin, direction * wallDistance, Color.red);
                Debug.Log("Wall! STOP!");

                //DISABLE "FORWARD" CONTROL FROM PLAYER CONTROLLER

                //this will push back player for now, get rid when the disable forward is added in player controller
                Vector3 backOff = -transform.forward * 0.05f;
                transform.position += backOff;
            }
        }
        else
        {
            Debug.DrawRay(origin, direction * wallDistance, Color.green);
            //ENABLE "FORWARD" CONTROL IN PLAYER CONTROLLER
        }

    }

    private string GetPlayerViewSide(RaycastHit hit) //Finds the side the player is looking at
    {
        Vector3 toPlayer = (transform.position - hit.transform.position).normalized;
        toPlayer.y = 0;
        toPlayer.Normalize();
        Vector3 localDir = hit.transform.InverseTransformDirection(toPlayer);

        float absX = Mathf.Abs(localDir.x);
        float absZ = Mathf.Abs(localDir.z);

        if (absX > absZ)
        {
            return localDir.x > 0 ? "Right" : "Left";
        }
        else
        {
            return localDir.z > 0 ? "Front" : "Back";
        }
    }

    private void OnDrawGizmos() //Gizmos for inspector for visual input when testing
    {
        Gizmos.color = Color.yellow;
        Vector3 worldDir = transform.TransformDirection(transform.forward.normalized);
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
        Gizmos.DrawWireSphere(transform.position + worldDir * maxDistance, sphereRadius);
        Gizmos.DrawLine(transform.position, transform.position + worldDir * maxDistance);
    }
}
