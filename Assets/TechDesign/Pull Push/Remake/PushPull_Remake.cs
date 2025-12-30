using InputManager;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PushPull_Remake : MonoBehaviour
{

    public static PlayerManager playerManager;

    public GameObject Player;

    public GameObject MoveableObj;
    //PushPull_Remake self;
    //public PushPull_Remake MainPushObj;
    //public PushPull_Remake nearestPushable;
    //public PushPull_Remake[] allPushObjects;
    public PushPullMaster Master;

    public bool currentlyActive;

    Rigidbody rb;

    public PlayerMovement Movement;

    public PlayerInput moveInput;

    public InputAction moveAction;
    public InputAction moveAction2;
    public InputAction Interact;

    public TriggerScript_PlayerCheck[] triggers;
    public TriggerScript_PlayerCheck currentTrigger;

    public GameObject Direction;
    Transform newDirection;

    Quaternion newRot;

    public bool pushPulling = false;
    bool FirstRun = true;

    public bool active;
    bool objectFollow;

    //public int activeObjects = 0;

    //public float distance;
    //public float nearestDistance = 1000000;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        active = false;
        playerManager = Player.GetComponent<PlayerManager>();
        Movement = Player.GetComponent<PlayerMovement>();
        moveInput = Player.GetComponent<PlayerInput>();
        //self = MoveableObj.GetComponent<PushPull_Remake>();

        rb = MoveableObj.GetComponent<Rigidbody>();

        moveAction = moveInput.actions.FindAction("Move");
        moveAction2 = moveInput.actions.FindAction("PushPulling");
        Interact = moveInput.actions.FindAction("Interact");

        newDirection = Direction.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //AllPushObjects();
        if (active)
        {
            TriggerCheck();
            PushPulling();
            Movement.pushpulling = pushPulling;
            if (pushPulling)
            {
                rb.isKinematic = false;
                Player.transform.rotation = newRot;
            }
            else
            {
                rb.isKinematic = true;
            }

            if (currentTrigger.inTrigger == false)
            {
                pushPulling = false;
            }

            if (Movement.pushpulling)
            {
                Player.transform.rotation = newRot;
            }
        }
        else
        {
            if (pushPulling)
            {
                Player.transform.rotation = newRot;
            }
        }
        TriggerCheck();
    }

    void FixedUpdate() //for collision detection
    {
        if (active)
        {
            PushPulling();
            if (objectFollow && moveAction2.IsInProgress() && pushPulling)
            {
                rb.MovePosition(transform.position + Movement.move * Time.deltaTime);
            }

            if(Interact.IsPressed() && pushPulling)
            {
                Player.transform.rotation = newRot;
            }
        }
        else
        {
            if (currentTrigger.inTrigger)
            {
                //active = true;
            }

            if (pushPulling)
            {
                Player.transform.rotation = newRot;
            }
        }
        /*PushPulling();
        if (objectFollow && moveAction2.IsInProgress() && pushPulling)
        {
            rb.MovePosition(transform.position + Movement.move * Time.deltaTime);
        }*/
        //rb.MovePosition(transform.position + Movement.move * Time.deltaTime);
    }

    void PushPulling()
    {
        if (pushPulling && currentTrigger.inTrigger == true)
        {
            Player.transform.rotation = newRot;
            //Player.transform.position = currentTrigger.transform.position;
            if (Interact.IsPressed())
            {
                if (Interact.IsInProgress())
                {
                    //Player.transform.rotation = currentTrigger.transform.rotation;
                    Interact.Reset();
                    //MoveableObj.transform.parent = null;
                    objectFollow = false;
                    pushPulling = false;
                    active = false;
                    //Master.DeactivateNearest();

                }
                else
                {
                    pushPulling = false;
                }
            }
            Movement.PushPullMove();
            playerManager.moveAction = moveAction2;
            Debug.Log("Set");
            if (moveAction2.IsPressed())
            {
                Movement.newTransform = newDirection;
            }
        }
        else
        {
            if (Interact.IsPressed() && currentTrigger.inTrigger == true)
            {
                if (Interact.IsInProgress())
                {
                    newRot = currentTrigger.transform.rotation;
                    var pos = new Vector3 (currentTrigger.transform.position.x, 1.58f, currentTrigger.gameObject.transform.position.z);
                    Player.transform.rotation = newRot;
                    Player.transform.position = pos;
                    //Player.transform.position = 
                    Interact.Reset();
                    //MoveableObj.transform.parent = Player.transform;
                    objectFollow = true;
                    pushPulling = true;
                    //currentlyActive = true;
                }
                else
                {
                    
                    pushPulling = true;
                }
                //pushPulling= true;
            }
            else if (Movement.pushpulling == false)
            {
                Movement.PushPullStop();
                playerManager.moveAction = moveAction;
                Debug.Log("Reset");
            }
            //Movement.PushPullStop();
            //playerManager.moveAction = moveAction;
            //Debug.Log("Reset");

        }
    }

    void TriggerCheck()
    {
        if (FirstRun)
        {
            foreach (var trigger in triggers)
            {
                if (trigger.inTrigger)
                {
                    currentTrigger = trigger;
                    newDirection = currentTrigger.gameObject.transform;
                    FirstRun = false;
                }
            }
        }
        if (currentTrigger.inTrigger == false)
        {
            foreach (var trigger in triggers)
            {
                if (trigger.inTrigger)
                {
                    Debug.Log("InTrigger");
                    currentTrigger = trigger;
                    newDirection = currentTrigger.gameObject.transform;
                }
            }
        }
        
    }

    /*void AllPushObjects()
    {
        for (int i  = 0; i < allPushObjects.Length; i++)
        {
            distance = Vector3.Distance(Player.transform.position, allPushObjects[i].transform.position);

            if (distance < nearestDistance)
            {
                nearestPushable = allPushObjects[i];
                nearestDistance = distance;
            }
        }

        foreach (var obj in allPushObjects)
        {
            if (obj.active)
            {
                activeObjects++;
            }
            else
            {
                activeObjects--;
                if (activeObjects < 0)
                {
                    activeObjects = 0;
                }
            }
            /*if (obj.pushPulling == true && obj != self)
            {
                obj.active = false;
                //active = false;
            }
            else if (obj.pushPulling == false && obj == self)
            {
                active = true;
            }
            //obj.

            if (obj.currentTrigger.inTrigger && (nearestPushable = obj))
            {
                obj.active = true;
            }
            else
            {
                obj.active = false;
            }
        }
    }*/

    /*void Scrap()
    {
        playerManager.moveAction.ApplyBindingOverride(
                    new InputBinding
                    {
                        path = "<Keyboard>/Left",
                        overridePath = null
                    });
    }*/

}
