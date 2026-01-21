using Npc.AI.Movement;
using UnityEditor.Searcher;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class BodoController : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed = 6f;
    public float turnSpeed = 12f;
    public float acceleration = 20f;
    public float airControlMultiplier = 0.5f;

    [Header("Jump")] 
    public float jumpForce = 7f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;

    [Header("Gravity")] 
    public float gravity = -9.81f;
    public float gravityMultiplier = 2f;
    public float fallMultiplier = 2.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 horizontalVelocity;

    private float coyoteCounter;
    private float jumpBufferCounter;
    
    [Header("Relative cam")]
    public Transform cameraTransform;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleTimers();
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    void HandleTimers()
    {
        if (controller.isGrounded)
            coyoteCounter = coyoteTime; //coyote time meaning it allows player to jump very shortly after leaving ground
        else
            coyoteCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime; //jump cool down
        else
            jumpBufferCounter -= Time.deltaTime;
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); //downy
        float vertical = Input.GetAxisRaw("Vertical"); //uppy

        /*Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        float control = controller.isGrounded ? 1f : airControlMultiplier;

        Vector3 targetVelocity = inputDir * moveSpeed * control;

        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, acceleration * Time.deltaTime);

        controller.Move(horizontalVelocity * Time.deltaTime);

        // Rotate toward movement direction
        if (inputDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
        //BodoAnims.instance.Walk();
        */
        
        // Raw input
        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        // Camera-relative directions
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Convert input to camera space
        Vector3 moveDir = (camForward * inputDir.z + camRight * inputDir.x).normalized;

        float control = controller.isGrounded ? 1f : airControlMultiplier;

        Vector3 targetVelocity = moveDir * moveSpeed * control;

        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, acceleration * Time.deltaTime);

        controller.Move(horizontalVelocity * Time.deltaTime);

        // Rotate toward movement direction
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    void HandleJump()
    {
        if (jumpBufferCounter > 0 && coyoteCounter > 0)
        {
            velocity.y = jumpForce;
            jumpBufferCounter = 0;
            coyoteCounter = 0;
        }
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float gravityScale = velocity.y < 0 ? fallMultiplier : gravityMultiplier;

        velocity.y += gravity * gravityScale * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
