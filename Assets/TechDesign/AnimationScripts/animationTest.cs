using UnityEngine;

public class animationTest : MonoBehaviour
{
    //Animation anim;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();
        StartAnim();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartAnim()
    {
        Debug.Log("startanim");
        //anim.Play();
        animator.SetBool("Idle", false);
        Invoke("StopAnim", 2f);
    }
    void StopAnim()
    {
        Debug.Log("stopanim");
        //anim.Stop();
        animator.SetBool("Idle", true);
        // anim.
        Invoke("StartAnim", 2f);
    }
}
