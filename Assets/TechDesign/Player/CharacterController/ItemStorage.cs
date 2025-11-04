using Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class ItemStorage : MonoBehaviour
    {
        public static ItemStorage instance;

        [Header("Item Storage Sound Names - HAVE TO BE EXACT TO AUDIO CLIP NAME")]
        [SerializeField] private string PickUpSoundFileName;
        [SerializeField] private string PutDownSoundFileName;
        [SerializeField] private string SwapSoundFileName;

        [Header("Points that Items are Parented to")]   //Points in space to put items
        public GameObject putDownPoint;
        [SerializeField] GameObject trunkPoint, hatPoint, saddlePointRight, saddlePointLeft;

        [SerializeReference] InputActionAsset m_actionList;

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
            instance ??= this;
        
            itemsInStorage = new GameObject[3];
        }

        // passes an object into item storage to pick it up, and childs it to a storage point
        public void PickUp(GameObject thatObject)
        {
            if (itemsInStorage[(int)Storage.Trunk] == null)
            {
                itemsInStorage[(int)Storage.Trunk] = thatObject;

                itemsInStorage[(int)Storage.Trunk].transform.position = trunkPoint.transform.position;
                itemsInStorage[(int)Storage.Trunk].transform.parent = trunkPoint.transform;
                itemsInStorage[(int)Storage.Trunk].transform.rotation = new Quaternion(0,0,0,0);
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
                    itemsInStorage[(int)Storage.Trunk].transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else if (itemsInStorage[(int)Storage.BagLeft] == null)
                {
                    itemsInStorage[(int)Storage.BagLeft] = itemsInStorage[(int)Storage.Trunk];

                    itemsInStorage[(int)Storage.BagLeft].transform.position = saddlePointLeft.transform.position;
                    itemsInStorage[(int)Storage.BagLeft].transform.parent = saddlePointLeft.transform;

                    itemsInStorage[(int)Storage.Trunk] = thatObject;

                    itemsInStorage[(int)Storage.Trunk].transform.position = trunkPoint.transform.position;
                    itemsInStorage[(int)Storage.Trunk].transform.parent = trunkPoint.transform;
                    itemsInStorage[(int)Storage.Trunk].transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
            //PlaySoundPickUp();
        }

            // checks if item in trunk, if so check if there is nothing in put down place. If put down point is clear, put down item
        public void PutDown(GameObject thatobject)
        {
            if (itemsInStorage[0] != null)
            {
                QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore;
                Collider[] intersecting = Physics.OverlapSphere(putDownPoint.transform.position, 0.5f, -1, queryTriggerInteraction);
                Debug.Log(intersecting.Length);
                foreach (Collider coll in intersecting)
                {
                    Debug.Log(coll.gameObject.name);
                }
                if (intersecting.Length == 1 || intersecting.Length == 2)
                {
                    Debug.Log("Put down");
                    itemsInStorage[(int)Storage.Trunk].GetComponent<PickUpPutDownScript>().isPickedUp = false;
                    itemsInStorage[(int)Storage.Trunk].transform.position = putDownPoint.transform.position;
                    itemsInStorage[(int)Storage.Trunk].transform.parent = null;
                    itemsInStorage[(int)Storage.Trunk] = null;
                    //PlaySoundPutDown();
                }
            }
        }

        // swaps trunk and left bag items
        public void SwapItemLeft()
        {
            if (itemsInStorage[(int)Storage.Trunk] != null || itemsInStorage[(int)Storage.BagLeft] != null)
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
                //PlaySoundSwap();
            }
        }

        // swaps trunk and right bag items
        public void SwapItemRight()
        {
            if (itemsInStorage[(int)Storage.Trunk] != null || itemsInStorage[(int)Storage.BagRight] != null)
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
                //PlaySoundSwap();
            }
        }

        // if holding a hat, put it on head
        public void WearHat()
        {

                Debug.Log("Hat attempt");
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
                        //PlaySoundSwap();
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
                    //PlaySoundSwap();
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
        void PlaySoundPickUp()
        {
            Debug.Log("playsound");
            AudioManager.instance.PlayAudio(PickUpSoundFileName, transform.position, false, false, false, 1.0f, 1.0f, true, 0.75f, 1.25f, 128);
        }

        void PlaySoundPutDown()
        {
            Debug.Log("playsound");
            AudioManager.instance.PlayAudio(PutDownSoundFileName, transform.position, false, false, false, 1.0f, 1.0f, true, 0.75f, 1.25f, 128);
        }
        void PlaySoundSwap()
        {
            Debug.Log("playsound");
            AudioManager.instance.PlayAudio(SwapSoundFileName, transform.position, false, false, false, 1.0f, 1.0f, true, 0.75f, 1.25f, 128);
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
}

