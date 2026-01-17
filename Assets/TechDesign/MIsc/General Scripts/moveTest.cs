using UnityEngine;

public class moveTest : MonoBehaviour
{
    [SerializeField] int movespeed;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxspeed;
    [SerializeField] int rotationSpeed;
    Rigidbody rb;

    private void Update()
    {
        Walk();
        //Debug.Log("forward");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Walk()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("forward");
            rb.linearVelocity += transform.forward * movespeed;
        }
    }
}
