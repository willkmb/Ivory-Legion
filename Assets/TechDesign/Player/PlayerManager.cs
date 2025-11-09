using System;
//using AI;
using Player;
using SeismicSense;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManager
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;
        
        public PlayerInput movementInputSystem;
        
        //Action Keys
        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction interactAction;
        [HideInInspector] public InputAction destroyObjAction;
        [HideInInspector] public InputAction swapItemLeftAction;
        [HideInInspector] public InputAction swapItemRightAction;
        [HideInInspector] public InputAction swapHatAction;
        [HideInInspector] public InputAction seismicSenseAction;
        
        //Cooldown Bool
        private bool _interactOffCooldown;
        [HideInInspector] public bool seismicOffCooldown = true;
        [HideInInspector] public bool swapItemLeftOffCooldown;
        [HideInInspector] public bool swapItemRightOffCooldown;
        [HideInInspector] public bool swappingHatOffCooldown = true;
        
        // Other Bool
        [HideInInspector] public bool movementAllowed = true;
        [HideInInspector] public bool interactionAllowed = true;
        [HideInInspector] public bool destroyChargeInProgress = false;
        
        // Scripts
        [HideInInspector] public DestructionScript currentDestructableObject;
        
        [Header("Values")]
        public float interactCooldown;
  
        
        private void Awake()
        {
            instance ??= this;

            
            interactionAllowed = true;
            _interactOffCooldown = true;
            seismicOffCooldown = true;
            swapItemLeftOffCooldown = true;
            swapItemRightOffCooldown = true;
            swappingHatOffCooldown = true;
        }

        private void Start()
        {
            moveAction = movementInputSystem.actions.FindAction("Move");
  
            interactAction = movementInputSystem.actions.FindAction("Interact");
            destroyObjAction = movementInputSystem.actions.FindAction("Destroy");
            
            swapItemLeftAction = movementInputSystem.actions.FindAction("Previous");
            swapItemRightAction = movementInputSystem.actions.FindAction("Next");
            swapHatAction = movementInputSystem.actions.FindAction("Swap Hat");
            
            seismicSenseAction = movementInputSystem.actions.FindAction("Seismic Sense");

            interactAction.performed += PickUpInteraction; // <- added by Emily, instead of interactAction.IsPressed() for interaction functionality
                // may be worth putting other single input code in separate callbackcontext functions? 
        }
        // ReSharper disable Unity.PerformanceAnalysis - Ignore This 
        private void Update()
        {
            // Movement
            if (movementAllowed) //doesn't work for controller
                if (moveAction.IsPressed())
                {
                    PlayerMovement.instance.Movement(moveAction.ReadValue<Vector2>());
                    PlayerMovement.instance.isWalking = true;
                    ElephantAnim.instance.Walk();
                }
                else if (moveAction.WasReleasedThisFrame())
                {
                    PlayerMovement.instance.isWalking = false;
                    ElephantAnim.instance.Idle();
                }

            // Interactions
            if (interactionAllowed)
            {
                // If not a destructable obj reset cooldown, and interact is on cooldown
                if (!destroyChargeInProgress && !PickUpPutDownScript.instance.isPickedUp && !_interactOffCooldown) // Set in "PlayerInteractScript.instance.CheckObjectInFront()"
                {
                    Invoke(nameof(InteractOffCoolDown), interactCooldown + interactCooldown* 0.05f);
                    return;
                }
                
                // DestroyChargeInProgress is set when an obj is interacted with - if destructable obj selected start charge
                // If the player stops holding down the button the charge attack is cancelled
                // And they have to interact with obj again to start the process
                
                if (currentDestructableObject != null  && destroyChargeInProgress && !PickUpPutDownScript.instance.isPickedUp)
                {
                    if (!destroyObjAction.IsInProgress())
                    {
                        destroyChargeInProgress = false;
                        currentDestructableObject.DestroyObj();
                        currentDestructableObject = null;
                    }
                }
            }
            // For inventory swapping
            if (swapItemLeftOffCooldown)
                if (swapItemLeftAction.IsPressed())
                {
                    swapItemLeftOffCooldown = false;
                    ItemStorage.instance.SwapItemLeft();
                    Invoke(nameof(SwapLeftOffDown), 0.25f); // Time based of elephant animation time (For Future)
                }
            // For inventory swapping
            if (swapItemRightOffCooldown)
                if (swapItemRightAction.IsPressed())
                {
                    swapItemRightOffCooldown = false;
                    ItemStorage.instance.SwapItemRight();
                    Invoke(nameof(SwapRightOffDown), 0.25f); // Time based of elephant animation time (For Future)
                }
            // For Hat Swapping
            if (swappingHatOffCooldown)
                if (swapHatAction.IsPressed())
                {
                    swappingHatOffCooldown = false;
                    ItemStorage.instance.WearHat();
                    Invoke(nameof(SwapHatOffCoolDown), 0.25f); // Time based of elephant animation time (For Future)
                }

            //Seismic Sense
            if (seismicOffCooldown)
                if (seismicSenseAction.IsPressed())
                {
                    ElephantAnim.instance.Seismic();
                    //Seismic Sense Stuff
                    SeismicSenseScript.instance.Reset(); // Resets the particles to center of player 
                    seismicOffCooldown = false;
                    SeismicSenseScript.instance.inProgress = true; // Allows particles of SS to start expanding
                    
                    SeismicSenseScript.instance.StartPulse();
                }
            
        }

        public void setMovementAllowed(bool allowed)
        {
            movementAllowed = allowed;
            if (allowed) moveAction.Enable(); else moveAction.Disable();
        }

        void PickUpInteraction(InputAction.CallbackContext context)
        {
            if (interactionAllowed)
            {
                if (_interactOffCooldown)
                {
                    //if (NpcTalkTrigger.instance.inTrigger)
                    //{
                       // _interactOffCooldown = false;

                        //NpcTalkTrigger.instance.Interact();
                        //return;
                    //}

                    //Debug.Log("Checking in front");
                    PlayerInteractScript.instance.CheckObjectInFront();

                    // If not a npc or no obj picked up, try pushing obj
                    if (!PickUpPutDownScript.instance.isPickedUp)
                    {
                        PushController.instance.TryPush();
                    }
                }
                Invoke(nameof(InteractOffCoolDown), interactCooldown + interactCooldown * 0.05f);
            }
        }
    
        public void InteractOffCoolDown()
        {
            _interactOffCooldown = true;
        }
        /// 
        private void SwapLeftOffDown()
        {
            swapItemLeftOffCooldown = true;
        }
        private void SwapRightOffDown()
        {
            swapItemRightOffCooldown = true;
        }
        private void SwapHatOffCoolDown()
        {
            swappingHatOffCooldown = true;
        }
    }
    /* Commented out and moved to separate function below <- by Emily
                if (_interactOffCooldown)
                    if (interactAction.IsPressed())
                    {

                        if(NpcTalkTrigger.instance.inTrigger)
                        {
                            _interactOffCooldown = false;

                            NpcTalkTrigger.instance.Interact();
                            return;
                        }
                        PlayerInteractScript.instance.CheckObjectInFront();
                    }

                // Drops item if the player has one in truck
                if (interactAction.IsPressed() && PickUpPutDownScript.instance.isPickedUp) <- moved drop/ put down item function from PickUpPutDownScript to ItemStorage script
                {
                   Debug.Log("drop item");
                    PickUpPutDownScript.instance.DropItems();
                    Invoke(nameof(InteractOffCoolDown), interactCooldown + interactCooldown* 0.05f);
                    return;
                }
                */

}

