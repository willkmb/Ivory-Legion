using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public bool isHat;
    public bool isPickedUp;
    TriggerScript trigger;
    GameObject player;
    PutDownScript PutDownScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trigger = GetComponentInChildren<TriggerScript>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isPickedUp == false)
        {
            PutDownScript = GetComponentInParent<PutDownScript>();
        }
        */
        //InitiatePickUpPutDown();
    }

    void InitiatePickUpPutDown()
    {
        if (trigger.inTrigger == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isPickedUp)
                {
                    player.GetComponent<ItemStorage>().PickUp(this.gameObject);
                    PutDownScript.ItemHere = null;
                    isPickedUp = true;
                }
            }
        }
    }


}
