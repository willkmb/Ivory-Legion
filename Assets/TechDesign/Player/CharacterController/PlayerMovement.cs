using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;                      //THIS IS USING THE OLD INPUT SYSTEM!!!!!!!

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement instance;
    
        [Header("References")]
        private CharacterController controller;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private ParticleSystem walkParticles1;
        [SerializeField] private ParticleSystem walkParticles2;

        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float turningSpeed = 10f;
        [SerializeField] private float gravity = 9.8f;

        private float verticalVelocity;

        [Header("Input")]
        private float moveInput;
        private float turnInput;

        public bool isWalking; // - Emily, particles
        private bool isParticlesPlaying;// - Emily, particles
        private void Awake()
        {
            instance ??= this;
        }

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (isWalking) // - Emily, particles
            {
                if (!isParticlesPlaying)
                {
                    StartWalkParticles();
                }
            }
            else
            {
                if (isParticlesPlaying)
                {
                    StopWalkParticles();                    
                }
            }
            //  InputManagement();
            // Movement();
        }
        // Move 4 directions
        public void Movement(Vector2 input)
        {
            // Get movement direction relative to camera
            Vector3 inputDirection = new Vector3(input.x, 0, input.y).normalized;

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

        public void InputManagement()
        {
            moveInput = Input.GetAxis("Vertical");
            turnInput = Input.GetAxis("Horizontal");
        }

        public void StartWalkParticles() // - Emily, particles
        {
            walkParticles1.Play();
            walkParticles2.Play();
            isParticlesPlaying = true;
        }
        public void StopWalkParticles() // - Emily, particles
        {
            walkParticles1.Stop();
            walkParticles2.Stop();
            isParticlesPlaying = false;
        }
    }
}


