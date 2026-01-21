using Unity.VisualScripting;
using UnityEngine;

public class PushPullMaster : MonoBehaviour
{
    public PushPull_Remake[] allPushObjects;
    public PushPull_Remake nearestPushable;

    [SerializeField] private GameObject hitObj;
    [SerializeField] private PushPull_Remake hitPush;

    public GameObject Player;

    //public bool CurrentlyActive;

    //public int activeObjects = 0;

    public float distance;
    public float nearestDistance = 1000000;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetActivePushable();

        var ray = new Ray(Player.transform.position, Player.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            hitObj = hit.transform.gameObject;
            hitPush = hitObj.GetComponent<PushPull_Remake>();
        }
    }


    void SetActivePushable()
    {
        /*foreach (var obj in allPushObjects)
        {
            if (obj.currentTrigger.inTrigger && activeObjects <= 2)
            {
                nearestPushable = obj;
                nearestPushable.active = true;
            }

            if (obj != nearestPushable)
            {
                nearestPushable.active = false;
            }
        }*/

        foreach (var obj in allPushObjects)
        {
            if (obj == hitPush)
            {
                nearestPushable = obj;
                nearestPushable.active = true;
            }
            else
            {
                obj.active = false;
            }
        }

    }

    /*public void DeactivateNearest()
    {
        //nearestPushable = null;
    }*/


}
