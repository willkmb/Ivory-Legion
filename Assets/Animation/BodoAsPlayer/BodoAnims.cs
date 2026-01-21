using UnityEngine;

public class BodoAnims : MonoBehaviour
{
    public static BodoAnims instance;
    Animator anim;
    Animator RunToIdle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void awake()
    {
        instance ??= this;
    }

    public void Walk()
    {
        anim.SetBool("IsMove", true);
    }

    public void Idle()
    {
        anim.SetBool("IsMove", false);
    }
}
