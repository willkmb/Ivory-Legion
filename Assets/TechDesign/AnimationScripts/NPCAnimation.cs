using UnityEngine;
using Npc.AI;

public class NPCAnimation : MonoBehaviour
{
    NpcManager npcManager;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcManager = GetComponent<NpcManager>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAnim();
    }

    public void ChangeAnim()
    {
        if (npcManager.stateSaver == NpcState.Idle || npcManager.stateSaver == NpcState.TalkingToPlayer)
        {
            Debug.Log("Idle");
            animator.SetBool("Idle", true);
        }

        else if (npcManager.stateSaver == NpcState.Walking || npcManager.stateSaver == NpcState.SetPathingWalking)
        {
            Debug.Log("Walking");
            animator.SetBool("Idle", false);
        }
    }
}
