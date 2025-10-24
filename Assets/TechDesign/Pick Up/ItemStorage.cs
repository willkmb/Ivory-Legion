using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    GameObject tempItemStorage0, tempItemStorage1, tempItemStorage2;
    GameObject hatOnHead;
    enum Storage : int
    {
        Trunk = 0, 
        BagLeft,
        BagRight,
    }

    [SerializeField] GameObject trunkPoint, hatPoint, saddlePoint1, saddlePoint2;

    [HideInInspector] public GameObject[] itemsInStorage;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemsInStorage = new GameObject[3];
    }

    // Update is called once per frame
    void Update()
    {

        if (itemsInStorage[(int)Storage.Trunk] != null)
        {
            itemsInStorage[(int)Storage.Trunk].transform.position = trunkPoint.transform.position;
            itemsInStorage[(int)Storage.Trunk].transform.parent = trunkPoint.transform;
        }

        if (itemsInStorage[(int)Storage.BagRight] != null)
        {
            itemsInStorage[(int)Storage.BagRight].transform.position = saddlePoint1.transform.position;
            itemsInStorage[(int)Storage.BagRight].transform.parent = trunkPoint.transform;
        }

        if (itemsInStorage[(int)Storage.BagLeft] != null)
        {
            itemsInStorage[(int)Storage.BagLeft].transform.position = saddlePoint2.transform.position;
            itemsInStorage[(int)Storage.BagLeft].transform.parent = trunkPoint.transform;
        }

        if (hatOnHead != null)
        {
            hatOnHead.transform.position = hatPoint.transform.position;
            hatOnHead.transform.parent = hatPoint.transform;
        }

        WearHat();
        RotateInvItems();

    }

    public void PickUp(GameObject thatObject)
    {
        if (itemsInStorage[(int)Storage.Trunk] == null)
        {
            itemsInStorage[(int)Storage.Trunk] = thatObject;
        }
        else
        {
            if (itemsInStorage[(int)Storage.BagRight] == null)
            {
                itemsInStorage[(int)Storage.BagRight] = itemsInStorage[(int)Storage.Trunk];
                itemsInStorage[(int)Storage.Trunk] = thatObject;
            }
            else if (itemsInStorage[(int)Storage.BagLeft] == null)
            {
                itemsInStorage[(int)Storage.BagLeft] = itemsInStorage[(int)Storage.Trunk];
                itemsInStorage[(int)Storage.Trunk] = thatObject;
            }

        }
        
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

    void RotateInvItems()
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
        

    }

    void WearHat()
    {
        if(itemsInStorage[(int)Storage.Trunk] != null)
        {
            if (itemsInStorage[(int)Storage.Trunk].GetComponent<PickUpScript>().isHat == true)
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

    }
}
