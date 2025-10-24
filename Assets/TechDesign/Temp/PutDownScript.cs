using UnityEngine;

public class PutDownScript : MonoBehaviour
{
    MechanicTriggerScript trigger;
    GameObject player;
    [SerializeField] GameObject PutDownPoint;
    public bool isPickedUp;
    public GameObject ItemHere;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trigger = GetComponentInChildren<MechanicTriggerScript>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (ItemHere == null && !isPickedUp)
        {
            ItemHere = GetComponentInChildren<PickUpScript>().gameObject;
        }
        else if (ItemHere != null)
        {
            ItemHere.transform.position = PutDownPoint.transform.position;
            ItemHere.transform.parent = PutDownPoint.transform;
        }
        InitiatePickUp();
        InitiatePutDown();
    }
    void InitiatePickUp()
    {
        if (trigger.inTrigger == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isPickedUp)
                {
                    player.GetComponent<ItemStorage>().PickUp(ItemHere);
                    ItemHere = null;
                    isPickedUp = true;
                }
            }
        }
    }
    void InitiatePutDown()
    {
        if (trigger.inTrigger == true)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (ItemHere == null)
                {
                    if (player.GetComponent<ItemStorage>().itemsInStorage[0] != null)
                    {
                        ItemHere = player.GetComponent<ItemStorage>().itemsInStorage[0];
                        player.GetComponent<ItemStorage>().itemsInStorage[0] = null;
                        isPickedUp = false;
                    }
                    
                }

            }
        }
    }
}
