using InputManager;
using Npc.AI;
using UnityEngine;
using TMPro;

namespace AI
{
public class NpcTalkTrigger : MonoBehaviour
{
    public static NpcTalkTrigger instance;
    
    TriggerScript triggerScript;
    public GameObject dialogueUI;
    Dialogue dialogue;
    Collider trigger;
    public bool inTrigger;
    private GameObject collidedWith;

    private void Awake()
    {
        instance ??= this;
    }
    void Start()
    {
        triggerScript = GetComponentInChildren<TriggerScript>();
        dialogueUI.SetActive(false);
    }
    
    public void Interact()
    {
        if (triggerScript != null)
        {
            if (inTrigger == true)
            {
                // Start dialogue system with that NPC
                    if(collidedWith != null)
                    {
                        dialogue = collidedWith.GetComponent<Dialogue>();
                        dialogue.ShowNextBranch();
                        TextMeshProUGUI text = collidedWith.GetComponent<NPCtrustValue>().text;
                        string opinion = collidedWith.GetComponent<NPCtrustValue>().opinionLevel;
                        text.text = "Opinion: " + opinion;
                        
                        // Change npc state
                        NpcManager npcManager = collidedWith.GetComponent<NpcManager>();
                        npcManager.npcState = NpcState.TalkingToPlayer;
                        npcManager.StateChanger();
                        
                        // Playesr Inputs
                        PlayerManager.instance.movementAllowed = false;
                    }
                    dialogueUI.SetActive(true);
                    dialogueUI.GetComponent<Animation>().Play();
                    Cursor.lockState = CursorLockMode.Confined;
                    //GameObject.FindGameObjectWithTag("Player").GetComponent<Move>().canMove = false;
            }
            else
            {
                dialogueUI.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            inTrigger = true;
            collidedWith = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            inTrigger = false;
            dialogue = collidedWith.GetComponent<Dialogue>();
            dialogue.branchIndex = dialogue.startIndex;
            collidedWith = null;
        }
    }
}
}
