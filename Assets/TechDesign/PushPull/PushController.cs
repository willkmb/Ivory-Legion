using UnityEngine;
using UnityEngine.InputSystem;

public class PushController : MonoBehaviour
{
    [Header("Ability to grab")]
    public float sphereRadius = 2f;
    public float maxDistance = 3f;
    public string pushableTag = "Pushable";
    public LayerMask pushableLayer;
    public float pushTimer = 1f;

    private GameObject pushedObj = null;

    [Header("Collisions")]
    public float wallDistance = 1.5f;
    public LayerMask collisionLayers;
    private float halfDepth;
    private float halfWidth;
    private bool whichSide;

    [Header("Input Bindings")]
    [SerializeField] InputAction grab;

    private void Awake()
    {
        grab.performed += GrabCheck;
    }

    private void OnEnable()
    {
        grab.Enable();
    }

    private void OnDisable()
    {
        grab.Disable();
    }

    void Update()
    {
        
    }

    private void GrabCheck(InputAction.CallbackContext context)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, transform.forward, maxDistance, pushableLayer);

        foreach (RaycastHit hit in hits)    //Finds the object being hit by raycast
        {
            Debug.Log(hit.transform.gameObject.name);
            if (hit.collider.CompareTag(pushableTag))   //Checks if object has the correct pushable tag
            {
                Debug.Log("Ray hit tag");
                //Activates GetPlayerViewSide function to find what side is being looked at and assigns that to whatSide
                string whatSide = GetPlayerViewSide(hit);
                Debug.Log("Player is looking at the " + whatSide + " side of the object.");
                MovePlayer(hit, whatSide);  //Starts MovePlayer function with what object is hit and what side is hit
            }
        }
    }

    private void MovePlayer(RaycastHit hit, string whatSide)    //Moves player to the object and the side that is being interacted with
    {
        Vector3 targetPos = hit.transform.position; //Finds position of moveable object

        if (whatSide == "Left")         //Moves player to "Left" side of object
        {
            targetPos = hit.transform.position - hit.transform.right;
            whichSide = true;   //Allows script to know the player is on the X axis
        }
        else if (whatSide == "Right")   //Moves player to "Right" side of object
        {
            targetPos = hit.transform.position + hit.transform.right;
            whichSide = true;   //Allows script to know the player is on the X axis
        }
        else if (whatSide == "Front")   //Moves player to "Front" side of object
        {
            targetPos = hit.transform.position + hit.transform.forward;
            whichSide = false;  //Allows script to know the player is on the Y axis
        }
        else if (whatSide == "Back")    //Moves player to "Back" side of object
        {
            targetPos = hit.transform.position - hit.transform.forward;
            whichSide = false;  //Allows script to know the player is on the Y axis
        }

        PushObject(); //Push Object starts after the player has been moved
    }

    private void PushObject()
    {
        //Finds the object being pushed again
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, transform.forward, maxDistance, pushableLayer);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag(pushableTag))
            {
                pushedObj = hit.collider.gameObject;

                Collider col = pushedObj.GetComponent<Collider>();

                if (col != null)
                {
                    halfWidth = col.bounds.extents.x;
                    halfDepth = col.bounds.extents.z;
                }

                break;
            }
        }
    }

    private void ActivateCollisionForward()
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
}
