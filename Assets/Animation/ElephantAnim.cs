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
    
    public void Default() 
    {
        anim.SetBool("isIdle", true);
        anim.SetBool("isWalking", false);
        anim.SetBool("canSeismic", false);
        anim.SetBool("canStomp", false);
        anim.SetBool("canPush", false);
        anim.SetBool("pickUp", false);
    }
    
    public void Walk()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);
    }
    
    public void Idle()
    {
        anim.SetBool("isWalking", false);
        Default();
        
    }

    public void Seismic()
    {
        anim.SetBool("canSeismic", true);
        anim.SetTrigger("canSeismicT"); 
        //{
          //  Default();
        //}
        
        Debug.Log("SEISMIC ANIM");
    }

    public void Push()
    {
        anim.SetTrigger("canPushT");
        {
            Default();
        }
        
    }

    public void Stomp()
    {
        //need add reference
        anim.SetTrigger("canStompT");
        anim.SetBool("canStop", true);
        {
            Default();
        }
        //Debug.Log("STOMP ANIM");
    }

    public void Pickup()
    {
        anim.SetBool("pickUp", true);
        anim.SetTrigger("pickUpT");
    }

    public void Putdown()
    {
        anim.SetBool("putDown", true);
        anim.SetTrigger("putDownT");
        //{
          //  Default();
        //}
    }
}
