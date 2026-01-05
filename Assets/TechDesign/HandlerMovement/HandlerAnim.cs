using UnityEngine;

public class HandlerAnim : MonoBehaviour
{
    HandlerMoveScript handlerMove;
    Animator animator;
    bool isIdle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handlerMove = GetComponent<HandlerMoveScript>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeHandlerAnim();
    }

    void ChangeHandlerAnim()
    {
        if (handlerMove.moving)
        {
            if (isIdle)
            {
                //Debug.Log("Idle");
                animator.SetBool("Idle", false);
                isIdle = false;
            }
        }
        else
        {
            if (!isIdle)
            {
                //Debug.Log("Idle");
                animator.SetBool("Idle", true);
                isIdle = true;
            }
        }
    }
}
