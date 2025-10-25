using UnityEngine;
using UnityEngine.InputSystem;

public class ItemStorage : MonoBehaviour
{
    [Header("Points that Items are Parented to")]   //Points in space to put items
    public GameObject putDownPoint;
    [SerializeField] GameObject trunkPoint, hatPoint, saddlePointRight, saddlePointLeft;

    [Header("Input Bindings")]  //Input Bindings
    [SerializeField] InputAction hatOn;
    [SerializeField] InputAction swapLeft;    
    [SerializeField] InputAction swapRight;    


    //Storage of item GameObjects
    GameObject hatOnHead;
    [HideInInspector] public GameObject[] itemsInStorage;
    GameObject tempItemStorage0, tempItemStorage1, tempItemStorage2;
    
    //Changes array int to a word for easier readability
    enum Storage : int
    {
        Trunk = 0, 
        BagLeft,
        BagRight,
    }


    private void Awake()
    {
        //set up input action functionality
        hatOn.performed += WearHat;
        swapLeft.performed += SwapItemLeft;
        swapRight.performed += SwapItemRight;

        itemsInStorage = new GameObject[3];
    }
    private void OnEnable()
    {
        //enables inputs
        //hatOn.AddBinding("<Keyboard>/H");
        hatOn.Enable();
        swapLeft.Enable();
        swapRight.Enable();
    }
    private void OnDisable()
    {
        //disables inputs
        hatOn.Disable();
        swapLeft.Disable();
        swapRight.Disable();
    }

    // passes an object into item storage to pick it up, and childs it to a storage point
    public void PickUp(GameObject thatObject)
    {
        if (itemsInStorage[(int)Storage.Trunk] == null)
        {
            itemsInStorage[(int)Storage.Trunk] = thatObject;

            itemsInStorage[(int)Storage.Trunk].transform.position = trunkPoint.transform.position;
            itemsInStorage[(int)Storage.Trunk].transform.parent = trunkPoint.transform;
        }
        else
        {
            if (itemsInStorage[(int)Storage.BagRight] == null)
            {
                itemsInStorage[(int)Storage.BagRight] = itemsInStorage[(int)Storage.Trunk];

                itemsInStorage[(int)Storage.BagRight].transform.position = saddlePointRight.transform.position;
                itemsInStorage[(int)Storage.BagRight].transform.parent = saddlePointRight.transform;

                itemsInStorage[(int)Storage.Trunk] = thatObject;

                itemsInStorage[(int)Storage.Trunk].transform.position = trunkPoint.transform.position;
                itemsInStorage[(int)Storage.Trunk].transform.parent = trunkPoint.transform;
            }
            else if (itemsInStorage[(int)Storage.BagLeft] == null)
            {
                itemsInStorage[(int)Storage.BagLeft] = itemsInStorage[(int)Storage.Trunk];

                itemsInStorage[(int)Storage.BagLeft].transform.position = saddlePointLeft.transform.position;
                itemsInStorage[(int)Storage.BagLeft].transform.parent = saddlePointLeft.transform;

                itemsInStorage[(int)Storage.Trunk] = thatObject;

                itemsInStorage[(int)Storage.Trunk].transform.position = trunkPoint.transform.position;
                itemsInStorage[(int)Storage.Trunk].transform.parent = trunkPoint.transform;
            }

        }
        
    }

    // swaps trunk and left bag items
    void SwapItemLeft(InputAction.CallbackContext context)
    {
        tempItemStorage1 = itemsInStorage[(int)Storage.Trunk];
        itemsInStorage[(int)Storage.Trunk] = itemsInStorage[(int)Storage.BagLeft];
        itemsInStorage[(int)Storage.BagLeft] = tempItemStorage1;
        tempItemStorage1 = null;

        if (itemsInStorage[(int)Storage.Trunk] != null)
        {
            itemsInStorage[(int)Storage.Trunk].transform.position = trunkPoint.transform.position;
            itemsInStorage[(int)Storage.Trunk].transform.parent = trunkPoint.transform;
        }

        if (itemsInStorage[(int)Storage.BagLeft] != null)
        {
            itemsInStorage[(int)Storage.BagLeft].transform.position = saddlePointLeft.transform.position;
            itemsInStorage[(int)Storage.BagLeft].transform.parent = saddlePointLeft.transform;
        }

    }

    // swaps trunk and right bag items
    void SwapItemRight(InputAction.CallbackContext context)
    {
        tempItemStorage1 = itemsInStorage[(int)Storage.Trunk];
        itemsInStorage[(int)Storage.Trunk] = itemsInStorage[(int)Storage.BagRight];
        itemsInStorage[(int)Storage.BagRight] = tempItemStorage1;
        tempItemStorage1 = null;

        if (itemsInStorage[(int)Storage.Trunk] != null)
        {
            itemsInStorage[(int)Storage.Trunk].transform.position = trunkPoint.transform.position;
            itemsInStorage[(int)Storage.Trunk].transform.parent = trunkPoint.transform;
        }

        if (itemsInStorage[(int)Storage.BagRight] != null)
        {
            itemsInStorage[(int)Storage.BagRight].transform.position = saddlePointRight.transform.position;
            itemsInStorage[(int)Storage.BagRight].transform.parent = saddlePointRight.transform;
        }

    }

    // if holding a hat, put it on head
    void WearHat(InputAction.CallbackContext context)
    {

        if (itemsInStorage[(int)Storage.Trunk] != null)
        {
            if (itemsInStorage[(int)Storage.Trunk].GetComponent<PickUpPutDownScript>().isHat == true)
            {
                if (hatOnHead == null)
                {
                    hatOnHead = itemsInStorage[(int)Storage.Trunk];
                    itemsInStorage[(int)Storage.Trunk] = null;
                    hatOnHead.transform.position = hatPoint.transform.position;
                    hatOnHead.transform.parent = hatPoint.transform;
                }
            }
        }
        else
        {
            if (hatOnHead != null)
            {
                itemsInStorage[(int)Storage.Trunk] = hatOnHead;
                hatOnHead = null;
                itemsInStorage[(int)Storage.Trunk].transform.position = trunkPoint.transform.position;
                itemsInStorage[(int)Storage.Trunk].transform.parent = trunkPoint.transform;
            }
        }

//usused code
        /*
        if(itemsInStorage[(int)Storage.Trunk] != null)
        {
            if (itemsInStorage[(int)Storage.Trunk].GetComponent<PickUpPutDownScript>().isHat == true)
            {
                if (Input.GetKeyDown(KeyCode.H))
                {
                    if (hatOnHead == null)
                    {
                        hatOnHead = itemsInStorage[(int)Storage.Trunk];
                        itemsInStorage[(int)Storage.Trunk] = null;
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (hatOnHead != null)
                {
                    itemsInStorage[(int)Storage.Trunk] = hatOnHead;
                    hatOnHead = null;
                }
            }
        }
        */
    }

    /*
    public void PutDown()
    {
        if (itemsInStorage[(int)Storage.Trunk] != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                itemsInStorage[(int)Storage.Trunk].transform.parent = null;
                itemsInStorage[(int)Storage.Trunk].gameObject.GetComponent<PickUpScript>().isPickedUp = false;
            }
        }
    }*/

    void RotateInvItems(InputAction.CallbackContext context)
    {
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tempItemStorage1 = itemsInStorage[0];
            itemsInStorage[0] = itemsInStorage[1];
            itemsInStorage[1] = tempItemStorage1;
            tempItemStorage1 = null;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tempItemStorage1 = itemsInStorage[0];
            itemsInStorage[0] = itemsInStorage[2];
            itemsInStorage[2] = tempItemStorage1;
            tempItemStorage1 = null;
        }
        */
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tempItemStorage0 = null;
            tempItemStorage1 = null;
            tempItemStorage2 = null;

            tempItemStorage0 = itemsInStorage[(int)Storage.Trunk];            
            tempItemStorage1 = itemsInStorage[(int)Storage.BagRight];
            tempItemStorage2 = itemsInStorage[(int)Storage.BagLeft];
            itemsInStorage[(int)Storage.Trunk] = tempItemStorage2;
            itemsInStorage[(int)Storage.BagRight] = tempItemStorage0;
            itemsInStorage[(int)Storage.BagLeft] = tempItemStorage1;


        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            tempItemStorage0 = null;
            tempItemStorage1 = null;
            tempItemStorage2 = null;

            tempItemStorage0 = itemsInStorage[(int)Storage.Trunk];            
            tempItemStorage1 = itemsInStorage[(int)Storage.BagRight];
            tempItemStorage2 = itemsInStorage[(int)Storage.BagLeft];
            itemsInStorage[(int)Storage.Trunk] = tempItemStorage1;
            itemsInStorage[(int)Storage.BagRight] = tempItemStorage2;
            itemsInStorage[(int)Storage.BagLeft] = tempItemStorage0;

        }
        
        */
    }
}
