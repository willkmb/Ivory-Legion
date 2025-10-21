using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;                      //THIS IS USING THE OLD INPUT SYSTEM!!!!!!!

public class Elephant_2 : MonoBehaviour
{
   
    
    [Header("References")]
    private CharacterController controller;
    [SerializeField] private Transform cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float turningSpeed = 10f;
    [SerializeField] private float gravity = 9.8f;

    private float verticalVelocity;

    [Header("Input")]
    private float moveInput;
    private float turnInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        InputManagement();
        Movement();
    }

    private void Movement()
    {
        // Get movement direction relative to camera
        Vector3 inputDirection = new Vector3(turnInput, 0, moveInput).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // Change input to world space relative to camera
            Vector3 moveDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * inputDirection;

            // Rotate player toward movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turningSpeed);

            // Move player in that direction
            Vector3 move = moveDirection * walkSpeed;
            move.y = VerticalForceCalculation();

            controller.Move(move * Time.deltaTime);
        }
        else
        {
            // Apply gravity even if not moving
            Vector3 move = new Vector3(0, VerticalForceCalculation(), 0);
            controller.Move(move * Time.deltaTime);
        }
    }

    private float VerticalForceCalculation()
    {
        if (controller.isGrounded)
            verticalVelocity = -1f;
        else
            verticalVelocity -= gravity * Time.deltaTime;

        return verticalVelocity;
    }

    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }
}

