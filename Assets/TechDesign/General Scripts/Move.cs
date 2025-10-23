using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] int movespeed;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxspeed;
    [SerializeField] int rotationSpeed;

    public bool canMove = true;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Walk();
            //Rotate();
        }
    }

    void Walk()
    {
        // Get current linear velocity
        Vector3 lv = rb.linearVelocity;

        if (lv.magnitude <= maxspeed)
        {
            Vector3 moveDir = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                Debug.Log("forward");
                moveDir += transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                Debug.Log("back");
                moveDir -= transform.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                Debug.Log("left");
                moveDir -= transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                Debug.Log("right");
                moveDir += transform.right;
            }

            if (moveDir != Vector3.zero)
            {
                moveDir.Normalize();
                rb.linearVelocity += moveDir * movespeed;
            }
            else
            {
                Debug.Log("no move");
                // stop horizontal movement, preserve vertical axis if needed
                rb.linearVelocity = new Vector3(0f, lv.y, 0f);
            }
        }
    }
}


