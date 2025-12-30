using UnityEngine;

public class TriggerScript_PlayerCheck : MonoBehaviour
{

    public bool inTrigger;

    public PushPullMaster Master;

    private void Start()
    {
        Master = FindAnyObjectByType<PushPullMaster>();
    }

    private void Update()
    {
       
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = true;
            //Master.activeObjects++;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = false;
            //Master.activeObjects--;
            /*if (Master.activeObjects < 0)
            {
                Master.activeObjects = 0;
            }*/
        }
    }
}
