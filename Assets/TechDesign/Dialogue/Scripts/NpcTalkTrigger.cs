using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Player;
using InputManager;
using Npc.AI;

public class NpcTalkTrigger : MonoBehaviour
{
    TriggerScript triggerScript;
    public GameObject dialogueUI;
    public GameObject bubbleUI;
    Dialogue dialogue;
    Collider trigger;
    public bool inTrigger;
    [HideInInspector] public GameObject collidedWith = null;

   // private bool bubbleEnabled = false;
    void Start()
    {
        triggerScript = GetComponentInChildren<TriggerScript>();
        dialogueUI.SetActive(false);
    }

    void Update()
    {
        Interact();
    }


    void Interact()
    {
        if (triggerScript != null)
        {
            if (inTrigger == true)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)) //&& !bubble.enabled;
                {
                    Debug.Log("Pressed");
                    if (collidedWith != null)
                    {
                        dialogue = collidedWith.GetComponentInParent<Dialogue>();
                        dialogue.ShowNextBranch();
                        TextMeshProUGUI text = collidedWith.GetComponentInParent<NPCtrustValue>().text;
                        string opinion = collidedWith.GetComponentInParent<NPCtrustValue>().opinionLevel;
                        text.text = "Opinion: " + opinion;
                    }
                    dialogueUI.SetActive(true);
                    dialogueUI.GetComponent<Animation>().Play();
                    Cursor.lockState = CursorLockMode.Confined;
                    ControllerCursor cursor = GameObject.Find("ContCursor").GetComponent<ControllerCursor>();
                    cursor.CursorState(true);

                    PlayerManager manager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
                    manager.movementAllowed = false;
                    manager.interactionAllowed = false;
                    manager.moveAction.Disable();
                    
                    
                    NpcManager npcManager = collidedWith.transform.GetComponent<NpcManager>();
                    if (npcManager != null)
                    {
                        npcManager.npcState = NpcState.TalkingToPlayer;
                        npcManager.StateChanger();
                    }
                }
            }
            else
            {
                exitDialogue();
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton2)) exitDialogue();
        }
    }

    void exitDialogue()
    {
        Debug.Log("ExitDialogue called — inTrigger=" + inTrigger);
        dialogueUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        ControllerCursor cursor = GameObject.Find("ContCursor").GetComponent<ControllerCursor>();
        cursor.CursorState(false);
        PlayerManager manager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        manager.movementAllowed = true;
        manager.interactionAllowed = true;
        manager.moveAction.Enable();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            inTrigger = true;
            collidedWith = other.gameObject;
            //if (collidedWith.GetComponent<BubbleScript>() != null && collidedWith.GetComponent<BubbleScript>().enabled)
            //{
                //bubbleEnabled = true;
                //collidedWith.GetComponent<BubbleScript>().pickLine();
                //bubbleUI.SetActive(true);
            //}
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            //if (collidedWith.GetComponent<BubbleScript>() != null)
            //{
            //bubbleEnabled = false;
            //bubbleUI.SetActive(false);
            //}
            dialogue = other.GetComponentInParent<Dialogue>();
            if (dialogue != null) { dialogue.branchIndex = dialogue.startIndex; }
            inTrigger = false;
            collidedWith = null;
        }
    }

}

