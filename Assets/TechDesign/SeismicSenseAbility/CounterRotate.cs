using UnityEngine;

public class CounterRotate : MonoBehaviour
{
    public Transform child;

    void Update()
    {
        child.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
    }
}
