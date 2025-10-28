using UnityEngine;
using UnityEngine.InputSystem;
using static Interfaces.Interfaces;

namespace Player {
public class PickUpPutDownScript : MonoBehaviour, IInteractable
{
    // variables in inspector
    [SerializeField] GameObject PutDownPoint;
    public bool isHat;

    // private variables
    GameObject player;
    public bool isPickedUp;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // on interact from player
    public void Interact()
    {
        if (!isPickedUp)    // if the item is not picked up then pick it up using ItemStorage PickUp Function on player
        {
            player.GetComponent<ItemStorage>().PickUp(this.gameObject);
            isPickedUp = true;
        }
        else    // if the item is picked up (ie in trunk) then check if area is clear and put it down
        {
            if (player.GetComponent<ItemStorage>().itemsInStorage[0] != null)
            {
                PutDownPoint = player.GetComponent<ItemStorage>().putDownPoint;
                Collider[] intersecting = Physics.OverlapSphere(PutDownPoint.transform.position, 0.5f);
                Debug.Log(intersecting.Length);
                if (intersecting.Length == 1 || intersecting.Length == 2)
                { 
                    player.GetComponent<ItemStorage>().itemsInStorage[0] = null;
                    isPickedUp = false;
                    transform.position = PutDownPoint.transform.position;
                    transform.parent = null;
                }
            }
        }
    }
    public void Activate()
    {
        throw new System.NotImplementedException();
    }
}
}
