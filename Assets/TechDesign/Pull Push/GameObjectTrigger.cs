using UnityEngine;

public class GameObjectTrigger : MonoBehaviour
{
    [Header("This object will trigger the trigger")]
    [Tooltip("Make sure triggerObject has a rigidbody")]
    public GameObject triggerObject;

    public bool Trigger;

    //Temp
    public GameObject destroyObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == triggerObject)
        {
            Trigger = true;

            //True
            Destroy(destroyObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == triggerObject)
        {
            Trigger = false;
        }
    }
}
