using System;
using InputManager;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElephantAnim : MonoBehaviour
{
    public static ElephantAnim instance;
    Animator anim;
    Animator Elephant_Anim_Contr;
    //PlayerManager PlayerManager;
    //private PlayerInput _playerInput;
    //public PlayerInput moveAction;
    //public InputActionReference moveAction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       anim = GetComponent<Animator>();
    }

    private void Awake()
    {
        instance ??= this;
    }

    public void Walk()
    {
        anim.SetBool("isWalking", true);
        //need to get input here to make the walking true
        //if (PlayerMovement.instance.isWalking == true)
        //{
        //
        //}
    }
    
    public void Idle()
    {
        anim.SetBool("isWalking", false);
        // if (PlayerMovement.instance.isWalking == false)
        // {
        //     //when no input make walking bool false to make idle anim play
        //     
        // }
        
    }
    
    
}
