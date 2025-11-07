using System;
using InputManager;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator _animator;
    private PlayerInput _Input;
    private InputAction _moveAction;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        //_animator.SetBool("IsWalking", true); // should switch to Walk
        //_animator.SetBool("IsWalking", false); // should switch to Idle
        //StartAnim();
    }

    private void Update()
    {
        if (_animator = null)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _animator.SetBool("IsWalking", true);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                _animator.SetBool("IsWalking", true);
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                _animator.SetBool("IsWalking", true);
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                _animator.SetBool("IsWalking", true);
            }
        }
        //Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        //Debug.Log("moveValue" + moveValue);
    }

    private void Awake()
    {
        // Grab the Animator on this same GameObject
        _animator = GetComponent<Animator>();
        
        //_Input = GetComponent<PlayerInput>();
        //_moveAction = _Input.actions["Move"];
    }


}

