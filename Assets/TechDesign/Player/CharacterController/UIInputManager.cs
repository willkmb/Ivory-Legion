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

        //Action Keys
         public InputAction navigateLeftAction;
         public InputAction navigateRightAction;
        [HideInInspector] public InputAction navigateUpAction;
        [HideInInspector] public InputAction navigateDownAction;
         public InputAction navigateInteractAction;
        
        public List<ChoiceButton> currentDialogueButtonsList;
        
        private void Awake()
        {
            instance ??= this;
        }
        private void Start()
        {
            currentlyInDialogue = false;
            
            navigateLeftAction = PlayerManager.instance.uiInputSystem.actions.FindAction("NAVIGATION_LEFT");
            Debug.Log(navigateLeftAction);
            navigateRightAction = PlayerManager.instance.uiInputSystem.actions.FindAction("NAVIGATION_RIGHT");
            navigateUpAction = PlayerManager.instance.uiInputSystem.actions.FindAction("NAVIGATION_UP");
            navigateDownAction = PlayerManager.instance.uiInputSystem.actions.FindAction("NAVIGATION_DOWN");
            navigateInteractAction = PlayerManager.instance.uiInputSystem.actions.FindAction("NAVIGATION_INTERACT");
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
                choice.Unselected();
            }
        }

        private void DialogueInteraction(InputAction.CallbackContext context)
        {
            Debug.Log("Dialogue Interaction");
            ChoiceButton choiceButton = currentDialogueButtonsList[currentDialogueBoxSelected];
            choiceButton.Interacted();
        }
        private void DialogueLeftMovement(InputAction.CallbackContext context)
        {
           Debug.Log("Left Movement");
            if (currentlyInDialogue)
            { 
                UnSelectDialogueBoxes();

                if (currentDialogueBoxSelected > 0)
                    currentDialogueBoxSelected -= 1; 
                
                ChoiceButton choiceButton = currentDialogueButtonsList[currentDialogueBoxSelected];
                choiceButton.Selected();
            }
        }

        private void DialogueRightMovement(InputAction.CallbackContext context)
        {
           Debug.Log("Right Movement");
            if (currentlyInDialogue)
            {
                UnSelectDialogueBoxes();
                
                if (currentDialogueBoxSelected < currentDialogueButtonsList.Count - 1)
                    currentDialogueBoxSelected += 1; 
                
                ChoiceButton choiceButton = currentDialogueButtonsList[currentDialogueBoxSelected];
                choiceButton.Selected();
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

