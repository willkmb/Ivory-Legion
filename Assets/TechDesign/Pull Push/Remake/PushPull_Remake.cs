using InputManager;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PushPull_Remake : MonoBehaviour
{

    public static PlayerManager playerManager;

    public GameObject Player;

    public GameObject MoveableObj;

    //public GameObject[] Raycasts;

    public GameObject[] Ray1;
    public GameObject[] Ray2;
    public GameObject[] Ray3;
    public GameObject[] Ray4;

    //private GameObject[] HitRays;


    public float range = 0;
    bool hasHit;

    [SerializeField] private GameObject hitObj1;
    [SerializeField] private GameObject hitObj2;
    [SerializeField] private GameObject hitObj3;
    [SerializeField] private GameObject hitObj4;

    [SerializeField] public TriggerScript_PlayerCheck triggerObj1;
    [SerializeField] public TriggerScript_PlayerCheck triggerObj2;
    [SerializeField] public TriggerScript_PlayerCheck triggerObj3;
    [SerializeField] public TriggerScript_PlayerCheck triggerObj4;

    private PushPull_Remake self;

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

    public int addedHit1;
    public int addedHit2;
    public int addedHit3;
    public int addedHit4;

    public bool ray1_1 = false;
    public bool ray1_2 = false;
    public bool ray1_3 = false;


    public bool ray2_1 = false;
    public bool ray2_2 = false;
    public bool ray2_3 = false;


    public bool ray3_1 = false;
    public bool ray3_2 = false;
    public bool ray3_3 = false;


    public bool ray4_1 = false;
    public bool ray4_2 = false;
    public bool ray4_3 = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        active = false;
        playerManager = Player.GetComponent<PlayerManager>();
        Movement = Player.GetComponent<PlayerMovement>();
        moveInput = Player.GetComponent<PlayerInput>();
        self = MoveableObj.GetComponent<PushPull_Remake>();

        rb = MoveableObj.GetComponent<Rigidbody>();

        moveAction = moveInput.actions.FindAction("Move");
        moveAction2 = moveInput.actions.FindAction("PushPulling");
        Interact = moveInput.actions.FindAction("Interact");

        newDirection = Direction.transform;

        hitObj1 = self.gameObject;
        hitObj2 = self.gameObject;
        hitObj3 = self.gameObject;
        hitObj4 = self.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTrigger.gameObject.activeSelf == false)
        {
            currentTrigger.inTrigger = false;
        }
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            active = false;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            active = true;
        }
    }

    void FixedUpdate() //for collision detection
    {
        if (active)
        {
            PushPulling();
            if (objectFollow && moveAction2.IsInProgress() && pushPulling)
            {
                rb.MovePosition(transform.position + Movement.move * Time.deltaTime);
                if (triggerObj1.inTrigger)
                {
                    rb.AddForce(transform.forward + Movement.move * Time.deltaTime);
                }

                if (triggerObj2.inTrigger)
                {
                    rb.AddForce(transform.right + Movement.move * Time.deltaTime);
                }

                if (triggerObj3.inTrigger)
                {
                    rb.AddForce(-transform.forward + Movement.move * Time.deltaTime);
                }

                if (triggerObj4.inTrigger)
                {
                    rb.AddForce(-transform.right + Movement.move * Time.deltaTime);
                }

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


        /////////////////////////////////////////////////////////////////////////
        // Raycast //
        /////////////////////////////////////////////////////////////////////////

        foreach (var objs in Ray1)
        {
            var ray1 = new Ray(objs.transform.position, objs.transform.forward);
            RaycastHit hit1;
            if (Physics.Raycast(ray1, out hit1, range))
            {
                hitObj1 = hit1.transform.gameObject;
                if (hit1.transform.gameObject.CompareTag("Untagged"))
                {
                    if (objs == Ray1[0])
                    {
                        if (ray1_1 == false)
                        {
                            addedHit1++;
                            ray1_1 = true;
                        }
                    }
                    if (objs == Ray1[1])
                    {
                        if (ray1_2 == false)
                        {
                            addedHit1++;
                            ray1_2 = true;
                        }
                    }
                    if (objs == Ray1[2])
                    {
                        if (ray1_3 == false)
                        {
                            addedHit1++;
                            ray1_3 = true;
                        }
                    }
                }

            }
            else
            {
                Debug.Log("working");
                //hitObj1 = self.gameObject;
                if (objs == Ray1[0])
                {
                    if(ray1_1 == true)
                    {
                        addedHit1--;
                        ray1_1 = false;
                    }

                }
                if (objs == Ray1[1])
                {
                    if (ray1_2 == true)
                    {
                        addedHit1--;
                        ray1_2 = false;
                    }

                }
                if (objs == Ray1[2])
                {
                    if (ray1_3 == true)
                    {
                        addedHit1--;
                        ray1_3 = false;
                    }

                }

            }
        }

        foreach (var objs in Ray2)
        {
            var ray1 = new Ray(objs.transform.position, objs.transform.forward);
            RaycastHit hit1;
            if (Physics.Raycast(ray1, out hit1, range))
            {
                hitObj1 = hit1.transform.gameObject;
                if (hit1.transform.gameObject.CompareTag("Untagged"))
                {
                    if (objs == Ray2[0])
                    {
                        if (ray2_1 == false)
                        {
                            addedHit2++;
                            ray2_1 = true;
                        }
                    }
                    if (objs == Ray2[1])
                    {
                        if (ray2_2 == false)
                        {
                            addedHit2++;
                            ray2_2 = true;
                        }
                    }
                    if (objs == Ray2[2])
                    {
                        if (ray2_3 == false)
                        {
                            addedHit2++;
                            ray2_3 = true;
                        }
                    }
                    //triggerObj2.gameObject.SetActive(false);
                }

            }
            else
            {
                if (objs == Ray2[0])
                {
                    if (ray2_1 == true)
                    {
                        addedHit2--;
                        ray2_1 = false;
                    }

                }
                if (objs == Ray2[1])
                {
                    if (ray2_2 == true)
                    {
                        addedHit2--;
                        ray2_2 = false;
                    }

                }
                if (objs == Ray2[2])
                {
                    if (ray2_3 == true)
                    {
                        addedHit2--;
                        ray2_3 = false;
                    }

                }
                //triggerObj2.gameObject.SetActive(true);
            }
        }

        foreach (var objs in Ray3)
        {
            var ray1 = new Ray(objs.transform.position, objs.transform.forward);
            RaycastHit hit1;
            if (Physics.Raycast(ray1, out hit1, range))
            {
                hitObj1 = hit1.transform.gameObject;
                if (hit1.transform.gameObject.CompareTag("Untagged"))
                {
                    if (objs == Ray3[0])
                    {
                        if (ray3_1 == false)
                        {
                            addedHit3++;
                            ray3_1 = true;
                        }
                    }
                    if (objs == Ray3[1])
                    {
                        if (ray3_2 == false)
                        {
                            addedHit3++;
                            ray3_2 = true;
                        }
                    }
                    if (objs == Ray3[2])
                    {
                        if (ray3_3 == false)
                        {
                            addedHit3++;
                            ray3_3 = true;
                        }
                    }

                }

            }
            else
            {
                if (objs == Ray3[0])
                {
                    if (ray3_1 == true)
                    {
                        addedHit3--;
                        ray3_1 = false;
                    }

                }
                if (objs == Ray3[1])
                {
                    if (ray3_2 == true)
                    {
                        addedHit3--;
                        ray3_2 = false;
                    }

                }
                if (objs == Ray3[2])
                {
                    if (ray3_3 == true)
                    {
                        addedHit3--;
                        ray3_3 = false;
                    }

                }
            }
        }

        foreach (var objs in Ray4)
        {
            var ray1 = new Ray(objs.transform.position, objs.transform.forward);
            RaycastHit hit1;
            if (Physics.Raycast(ray1, out hit1, range))
            {
                hitObj1 = hit1.transform.gameObject;
                if (hit1.transform.gameObject.CompareTag("Untagged"))
                {
                    if (objs == Ray4[0])
                    {
                        if (ray4_1 == false)
                        {
                            addedHit4++;
                            ray4_1 = true;
                        }
                    }
                    if (objs == Ray4[1])
                    {
                        if (ray4_2 == false)
                        {
                            addedHit4++;
                            ray4_2 = true;
                        }
                    }
                    if (objs == Ray4[2])
                    {
                        if (ray4_3 == false)
                        {
                            addedHit4++;
                            ray4_3 = true;
                        }
                    }
                    //triggerObj4.gameObject.SetActive(false);
                }

            }
            else
            {
                if (objs == Ray4[0])
                {
                    if (ray4_1 == true)
                    {
                        addedHit4--;
                        ray4_1 = false;
                    }

                }
                if (objs == Ray4[1])
                {
                    if (ray4_2 == true)
                    {
                        addedHit4--;
                        ray4_2 = false;
                    }

                }
                if (objs == Ray4[2])
                {
                    if (ray4_3 == true)
                    {
                        addedHit4--;
                        ray4_3 = false;
                    }

                }
            }
        }

        if (addedHit1 >= 1)
        {
            triggerObj1.gameObject.SetActive(false);
        }
        else
        {
            triggerObj1.gameObject.SetActive(true);
            addedHit1 = 0;
        }

        if (addedHit2 >= 1)
        {
            triggerObj2.gameObject.SetActive(false);
        }
        else
        {
            triggerObj2.gameObject.SetActive(true);
            addedHit2 = 0;
        }

        if (addedHit3 >= 1)
        {
            triggerObj3.gameObject.SetActive(false);
        }
        else
        {
            triggerObj3.gameObject.SetActive(true);
            addedHit3 = 0;
        }

        if (addedHit4 >= 1)
        {
            triggerObj4.gameObject.SetActive(false);
        }
        else
        {
            triggerObj4.gameObject.SetActive(true);
            addedHit4 = 0;
        }

        ///1///
        /*
        var ray1 = new Ray(Ray1.transform.position, Ray1.transform.forward);
        RaycastHit hit1;
        if (Physics.Raycast(ray1, out hit1, range, LayerMask.GetMask("Default")))
        {
            Debug.DrawRay(Ray2.transform.position, Ray2.transform.forward, Color.green);
            hitObj1 = hit1.transform.gameObject;
            if (hit1.transform.gameObject.CompareTag("Environment"))
            {
                triggerObj1.gameObject.SetActive(false);
            }

        }
        else
        {
            Debug.Log("working");
            //hitObj1 = self.gameObject;
            triggerObj1.gameObject.SetActive(true);
        }

        ///2///

        var ray2 = new Ray(Ray2.transform.position, Ray2.transform.forward);
        RaycastHit hit2;
        if (Physics.Raycast(ray2, out hit2, range, LayerMask.GetMask("Default")))
        {
            Debug.DrawRay(Ray2.transform.position, Ray2.transform.forward, Color.green);
            hitObj2 = hit2.transform.gameObject;
            if (hit2.transform.gameObject.CompareTag("Environment"))
            {
                triggerObj2.gameObject.SetActive(false);
            }

        }
        else
        {
            Debug.Log("working");
            //hitObj2 = self.gameObject;
            triggerObj2.gameObject.SetActive(true);
        }

        ///3///

        var ray3 = new Ray(Ray3.transform.position, Ray3.transform.forward);
        RaycastHit hit3;
        if (Physics.Raycast(ray3, out hit3, range, LayerMask.GetMask("Default")))
        {
            Debug.DrawRay(Ray3.transform.position, Ray3.transform.forward, Color.green);
            hitObj3 = hit3.transform.gameObject;
            if (hit3.transform.gameObject.CompareTag("Environment"))
            {
                triggerObj3.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("working");
            //hitObj3 = self.gameObject;
            triggerObj3.gameObject.SetActive(true);
        }

        ///4///

        var ray4 = new Ray(Ray4.transform.position, Ray4.transform.forward);
        RaycastHit hit4;
        if (Physics.Raycast(ray4, out hit4, range, LayerMask.GetMask("Default")))
        {
            Debug.DrawRay(Ray4.transform.position, Ray4.transform.forward, Color.green);
            hitObj4 = hit4.transform.gameObject;
            if (hit4.transform.gameObject.CompareTag("Environment"))
            {
                triggerObj4.gameObject.SetActive(false);
            }

        }
        else
        {
            Debug.Log("working");
            //hitObj4 = self.gameObject;
            triggerObj4.gameObject.SetActive(true);
        }*/

        /////// MAKE THEM SEPERATE!!! IT WILL FIX THE PROBLEM!!!///////

        /*foreach (var Rays in Raycasts)
        {
            var ray = new Ray(Rays.transform.position, Rays.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, range))
            {
                hitObj = hit.transform.gameObject;
                if (hitObj.CompareTag("Environment"))
                {
                    if (!hasHit)
                    {
                        hasHit = true;
                    }
                    //triggerObj.gameObject.SetActive(false);
                }

                if (hasHit)
                {
                    triggerObj = Rays.gameObject.GetComponentInChildren<TriggerScript_PlayerCheck>();
                    triggerObj.gameObject.SetActive(false);
                }

            }
            else
            {
                if (hasHit)
                {
                    hasHit= false;
                }
                if (!hasHit)
                {
                    triggerObj.gameObject.SetActive(true);
                }
            }
        }*/
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
                    Player.transform.localPosition = pos;
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
