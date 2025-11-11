using System.Collections.Generic;
using InputManager;
using Player.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class UIInputManager : MonoBehaviour
    {
        public static UIInputManager instance;
        public int currentDialogueBoxSelected;
        public bool currentlyInDialogue;
        public global::Dialogue dialogueInUse;

        //Action Keys
        public InputAction navigateLeftAction;
        public InputAction navigateRightAction;
        [HideInInspector] public InputAction navigateUpAction;
        [HideInInspector] public InputAction navigateDownAction;
        public InputAction navigateInteractAction;
        
        public List<ChoiceButton> currentDialogueButtonsList;
        public ChoiceButton currentDialogueButton;
        
        private void Awake()
        {
            instance ??= this;
        }
        private void Start()
        {
            currentlyInDialogue = false;
            
            navigateLeftAction = PlayerManager.instance.movementInputSystem.actions.FindAction("NAVIGATION_LEFT");
            Debug.Log(navigateLeftAction.bindings);
            navigateRightAction = PlayerManager.instance.movementInputSystem.actions.FindAction("NAVIGATION_RIGHT");
            navigateUpAction = PlayerManager.instance.movementInputSystem.actions.FindAction("NAVIGATION_UP");
            navigateDownAction = PlayerManager.instance.movementInputSystem.actions.FindAction("NAVIGATION_DOWN");
            navigateInteractAction = PlayerManager.instance.movementInputSystem.actions.FindAction("NAVIGATION_INTERACT");
           // navigateActionUp = PlayerManager.instance.inputSystem.actions.FindAction("Point/Up");
           
           navigateLeftAction.performed += DialogueLeftMovement;
           navigateRightAction.performed += DialogueRightMovement;
           navigateInteractAction.performed += DialogueInteraction;
        }

        public void ResetDialogueBoxNumber()
        {
            currentDialogueBoxSelected = 0;
        }

        private void UnSelectDialogueBoxes()
        {
            foreach (ChoiceButton choice in currentDialogueButtonsList)
            {
                choice.Unselected(); // Rests Visuals
            }
        }

        private void DialogueInteraction(InputAction.CallbackContext context)
        {
            Debug.Log("Dialogue Interaction");
            if (currentlyInDialogue)
            {
                //When interact is pressed, select the current dialogue box as the players dialogue choice
                currentDialogueButton = currentDialogueButtonsList[currentDialogueBoxSelected];
                currentDialogueButton.Interacted(); // Then continue onto next dialogue branch
            }
        }
        private void DialogueLeftMovement(InputAction.CallbackContext context)
        {
           Debug.Log("Left Movement");
           // Select Dialogue box to the left
            if (currentlyInDialogue)
            { 
                UnSelectDialogueBoxes(); // Resets UI Input variables to allow player movement again

                if (currentDialogueBoxSelected > 0)
                    currentDialogueBoxSelected -= 1; 
                
                currentDialogueButton = currentDialogueButtonsList[currentDialogueBoxSelected];
                currentDialogueButton.Selected(); // Changes visuals such as size
            }
        }

        private void DialogueRightMovement(InputAction.CallbackContext context)
        {
           Debug.Log("Right Movement");
            if (currentlyInDialogue)
            {
                UnSelectDialogueBoxes();     // Resets UI Input variables to allow player movement again
                
                if (currentDialogueBoxSelected < currentDialogueButtonsList.Count - 1)
                    currentDialogueBoxSelected += 1; 
                
                currentDialogueButton = currentDialogueButtonsList[currentDialogueBoxSelected];
                currentDialogueButton.Selected(); // Changes visuals such as size
            }
        }
        
        
        
        // Code that MIGHT be used for this ui movement
        
        // UI Inputs
        //InputBinding.MaskByGroup("Gamepad");
        // Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //  Debug.Log(navigateAction.ReadValue<Vector2>() * 100 * Time.deltaTime);
        //  Mouse.current.WarpCursorPosition(_mousePosition);
          
        // I hate this
          
        // if(navigateAction.inProgress || navigateActionUp.inProgress)
        // {
        //     Vector2 move = navigateAction.ReadValue<Vector2>();
        //
        //     Cursor.lockState = CursorLockMode.Confined;
        //     pos += move * ( 5 * Time.deltaTime);
        //     Debug.Log(pos);
        //
        //     // Mouse.current.WarpCursorPosition(pos);
        // }
    } 
}

