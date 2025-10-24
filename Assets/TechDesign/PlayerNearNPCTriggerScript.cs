using UnityEngine;

public class PlayerNearNPCTriggerScript : MonoBehaviour
{
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
