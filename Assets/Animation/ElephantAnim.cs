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
        anim.SetBool("isIdle", true);
        anim.SetBool("isWalking", false);
        // if (PlayerMovement.instance.isWalking == false)
        // {
        //     //when no input make walking bool false to make idle anim play
        //     
        // }
        
    }

    public void Seismic()
    {
        //anim.SetBool("idle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("canSeismic", true);
        Debug.Log("SEISMIC ANIM");
    }

    public void Push()
    {
        anim.SetBool("canPush", true);
        //Debug.Log("PUSHING ANIM");
        
        anim.SetBool("canPushing", false);
    }

    public void Stomp()
    {
        anim.SetBool("canStomp", true);
        Debug.Log("STOMP ANIM");
    }

    public void Pickup()
    {
        anim.SetBool("pickUpB", true);
        anim.SetTrigger("pickUpT");
    }

}
