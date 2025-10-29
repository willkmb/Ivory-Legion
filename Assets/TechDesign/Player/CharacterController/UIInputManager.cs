using System;
using InputManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Player
{
    public class UIInputManager : MonoBehaviour
    {
        public static UIInputManager instance;

        //Action Keys
        [HideInInspector] public InputAction navigateAction;
        [HideInInspector] public InputAction navigateActionUp;
        private Vector2 _mousePosition;
        
        private void Awake()
        {
            instance ??= this;
        }
        private void Start()
        {
            navigateAction = PlayerManager.instance.inputSystem.actions.FindAction("Point");
           // navigateActionUp = PlayerManager.instance.inputSystem.actions.FindAction("Point/Up");
            Debug.Log(navigateActionUp.name);
        }
        Vector2 pos = Vector2.zero;
        private void Update()
        {
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


        
        public void EnableUIControllerInputs()
        {
            navigateAction.Enable();
        }
    } 
}

