using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    //Camera follow
    [SerializeField] private Transform target;   // The player or object following
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = false;
    [SerializeField] private bool followZ = false;
    [SerializeField] private Vector3 offset;     // Optional positional offset
    [SerializeField] private float smoothSpeed = 5f;  // Optional smoothing

    //Camera zoom
    [SerializeField] private float zoomSeed = 50f; //speed of zoom
    [SerializeField] private float minFOV = 20;    //min zoom
    [SerializeField] private float maxFOV = 60f;   //max zoom

    public Camera Cam { get; private set; }

    //camera follow
    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = transform.position;

        if (followX)
            desiredPosition.x = target.position.x + offset.x;
        if (followY)
            desiredPosition.y = target.position.y + offset.y;
        if (followZ)
            desiredPosition.z = target.position.z + offset.z;

        // Smoothly interpolate for new position (this is optional )
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }

    //Camera zoom
    private void Start()
    {
        Cam = GetComponent<Camera>();
    }

    private void Update()
    {
        float zoomInput = 0f;

        //up arrow is zoom in and down arrow is zoom out
        if (Input.GetKey(KeyCode.UpArrow))
            zoomInput = -1f;
        else if (Input.GetKey(KeyCode.DownArrow))
            zoomInput = 1f;

        if (zoomInput != 0f)
        {
            Cam.fieldOfView += zoomInput * zoomSeed * Time.deltaTime;
            Cam.fieldOfView = Mathf.Clamp(Cam.fieldOfView, minFOV, maxFOV);
        }
    }
}