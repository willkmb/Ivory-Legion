using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicTriggerScript : MonoBehaviour
{
    Collider trigger;
    public bool inTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = false;
        }
    }
}
