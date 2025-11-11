using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    Collider trigger;
    public bool inTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            inTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "NPC")
        {
            inTrigger = false;
        }
    }
}
