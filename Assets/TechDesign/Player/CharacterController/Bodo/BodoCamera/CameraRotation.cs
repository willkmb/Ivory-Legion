using UnityEngine;
using Unity.Cinemachine;

public class CameraRotation : MonoBehaviour
{
    //Input stuff
    public float rotationSpeed = 120f;
    void LateUpdate()
    {
        //Input
        float input = 0f;

        if (Input.GetKey(KeyCode.Q))
            input = -1f;
        else if (Input.GetKey(KeyCode.E))
            input = 1f;

        transform.Rotate(Vector3.up, input * rotationSpeed * Time.deltaTime, Space.World);
    }
}

