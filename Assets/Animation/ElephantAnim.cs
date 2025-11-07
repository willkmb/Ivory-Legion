using InputManager;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElephantAnim : MonoBehaviour
{
    Animator anim;
    //Animator Elephant_Anim_Contr;
    PlayerManager PlayerManager;
    private PlayerInput _playerInput;
    //public PlayerInput moveAction;
    public InputActionReference moveAction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       anim = GetComponent<Animator>();
    }
    
    void Walk()
    {
        //need to get input here to make the walking true
        if (PlayerMovement.instance.isWalking == true)
        {
            anim.SetBool("isWalking", true);
        }
    }
    
    void Idle()
    {
        if (PlayerMovement.instance.isWalking == false)
        {
            //when no input make walking bool false to make idle anim play
            anim.SetBool("isWalking", false);
        }
        
    }
    
    
}
