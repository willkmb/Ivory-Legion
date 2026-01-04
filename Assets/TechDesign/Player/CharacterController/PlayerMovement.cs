using Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using InputManager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;                      //THIS IS USING THE OLD INPUT SYSTEM!!!!!!!

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement instance;

        [Header("Walk Sound Name - HAS TO BE EXACT TO AUDIO CLIP NAME")]
        [SerializeField] private string WalkSoundFileName;

        [Header("References")]
        private CharacterController controller;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private ParticleSystem walkParticles1;

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
        private bool isWalkSoundPlaying;// - Emily, sound

        public Transform newTransform; // Tommy, PushPull_Remake
        public bool pushpulling; // Tommy, PushPull_Remake

        public Vector3 move; //Tommy, PushPull_Remake

        private void Awake()
        {
            instance ??= this;
        }

        private void Start()
        {
            controller = GetComponent<CharacterController>();

            if (Camera.main != null) 
                cameraTransform = Camera.main.transform;

            newTransform = cameraTransform; //Tommy PushPull_Remake
        }

        private void Update()
        {
            if (PlayerManager.instance.inCutscene)
            {
                StopWalkParticles(); 
                return;
            }
            if (isWalking) // - Emily, particles
            {
                if (!isParticlesPlaying)
                {
                    StartWalkParticles();                    
                }
                if (!isWalkSoundPlaying)
                {
                    PlaySoundWalk();  
                    isWalkSoundPlaying = true; //<-- and ^ sounds for walking - Emily
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
            if (!PlayerManager.instance.movementAllowed || PlayerManager.instance.inCutscene)
                return;
            
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
                //Vector3 move = moveDirection * walkSpeed;
                move = moveDirection * walkSpeed;
                move.y = VerticalForceCalculation();

                controller.Move(move * Time.deltaTime);
            }
            else
            {
                // Apply gravity even if not moving
                //Vector3 move = new Vector3(0, VerticalForceCalculation(), 0);
                move = new Vector3(0, VerticalForceCalculation(), 0);
                controller.Move(move * Time.deltaTime);
            }
        }

        public void PushPullMove() // Tommy, PushPull_Remake
        {
            cameraTransform = newTransform; // allows for player movement to be detached from camera transform during push pull
        }

        public void PushPullStop()// Tommy, PushPull_Remake
        {
            cameraTransform = Camera.main.transform; // resets the camera transform so that player movement is based on camera positioning
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
            isParticlesPlaying = true;
        }
        public void StopWalkParticles() // - Emily, particles
        {
            walkParticles1.Stop();
            isParticlesPlaying = false;
        }
        void PlaySoundWalk() // - Emily, sounds
        {
            AudioManager.instance.PlayAudio(WalkSoundFileName, transform.position, false, false, false, 0.5f, 0.6f, true, 0.5f, 1f, 128);
            Invoke("SoundPlayingFalse", 0.75f);
        }

        void SoundPlayingFalse() // - Emily, sounds
        {
            isWalkSoundPlaying = false;
        }
    }
}


