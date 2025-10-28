using AI;
using Player;
using SeismicSense;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManager
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;
        
        public PlayerInput inputSystem;
        
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
        [HideInInspector] public bool swappingHatOffCooldown;
        
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
        }

        private void Start()
        {
            moveAction = inputSystem.actions.FindAction("Move"); // Gets "MOVE" In the Inputsystem
  
            interactAction = inputSystem.actions.FindAction("Interact");
            destroyObjAction = inputSystem.actions.FindAction("Destroy");
            
            swapItemLeftAction = inputSystem.actions.FindAction("Previous");
            swapItemRightAction = inputSystem.actions.FindAction("Next");
            swapHatAction = inputSystem.actions.FindAction("Swap Hat");
            
            seismicSenseAction = inputSystem.actions.FindAction("Seismic Sense");
        }
        // ReSharper disable Unity.PerformanceAnalysis - Ignore This 
        private void Update()
        {
            // Movement
            if (movementAllowed)
                if (moveAction.IsPressed())
                {
                    PlayerMovement.instance.Movement(moveAction.ReadValue<Vector2>());
                }
            // Interactions
            if (interactionAllowed)
            {
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
                        
                        // If not a destructable obj reset cooldown
                        if (!destroyChargeInProgress) // Set in "PlayerInteractScript.instance.CheckObjectInFront()"
                        {
                            Invoke(nameof(InteractOffCoolDown), interactCooldown + interactCooldown* 0.05f);
                            return;
                        }
                    }
                
                // DestroyChargeInProgress is set when an obj is interacted with - if destructable obj selected start charge
                // If the player stops holding down the button the charge attack is cancelled
                // And they have to interact with obj again to start the process
                
                if (currentDestructableObject != null  && destroyChargeInProgress)
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
                    //Seismic Sense Stuff
                    SeismicSenseScript.instance.Reset(); // Resets the particles to center of player 
                    seismicOffCooldown = false;
                    SeismicSenseScript.instance.inProgress = true; // Allows particles of SS to start expanding
                    SeismicSenseScript.instance.StartPulse();
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
}

