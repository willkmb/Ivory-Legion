using Cutscene;
using InputManager;
using Npc.AI;
using Player;
using SeismicSense;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle;
using UnityEngine;

public class NpcTalkTrigger : MonoBehaviour
{
    TriggerScript triggerScript;
    public GameObject dialogueUI;
    public GameObject bubbleUI;
    Dialogue dialogue;
    Collider trigger;
    public bool inTrigger;
    [HideInInspector] public GameObject collidedWith = null;
    private GameObject prompt;
    private HandlerMoveScript handler;
    bool talking = false;
    private bool bubbleEnabled = false;

    [Header("Animations")]
    Animator anim;
    void Start()
    {
        triggerScript = GetComponentInChildren<TriggerScript>();
        dialogueUI.SetActive(false);
        anim = GetComponentInChildren<Animator>();
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
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3) && !talking && collidedWith.GetComponentInParent<Dialogue>().enabled && !bubbleEnabled) //&& !bubble.enabled;
                {
                    talking = true;
                    Debug.Log("Pressed");
                    if (collidedWith != null)
                    {
                        dialogue = collidedWith.GetComponentInParent<Dialogue>();
                        dialogue.ShowNextBranch();
                        TextMeshProUGUI text = collidedWith.GetComponentInParent<NPCtrustValue>().text;
                        string opinion = collidedWith.GetComponentInParent<NPCtrustValue>().opinionLevel;
                        text.text = "Opinion: " + opinion;

                        prompt = collidedWith.GetComponentInParent<PromptScript>().thisPrompt;
                        handler = collidedWith.GetComponentInParent<HandlerMoveScript>();
                        if(handler != null) handler.enabled = false;
                        if(prompt != null) prompt.SetActive(false);
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

                    FindAnyObjectByType<SeismicSenseScript>().gameObject.SetActive(false);
                    anim.SetBool("isIdle", true);
                    anim.SetBool("isWalking", false);


                    NpcManager npcManager = collidedWith.transform.GetComponent<NpcManager>();
                    if (npcManager != null)
                    {
                        npcManager.npcState = NpcState.TalkingToPlayer;
                        npcManager.StateChanger();
                    }
                    CtDiaMoveCamera ctx = collidedWith.transform.GetComponent<CtDiaMoveCamera>(); // Moves camera if the interacted has the script on them
                    if (ctx != null)
                        ctx.MoveCamera();
                }
            }
            

            if (Input.GetKeyDown(KeyCode.JoystickButton2)) exitDialogue();
        }
    }

    public void exitDialogue()
    {
        Invoke("Move", 0.5f);
        Debug.Log("ExitDialogue called inTrigger=" + inTrigger);
        dialogueUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        ControllerCursor cursor = GameObject.Find("ContCursor").GetComponent<ControllerCursor>();
        cursor.CursorState(false);
        PlayerManager manager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        manager.movementAllowed = true;
        manager.interactionAllowed = true;
        manager.moveAction.Enable();
        talking = false;
        FindAnyObjectByType<SeismicSenseScript>().gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            inTrigger = true;
            collidedWith = other.gameObject;
            if (collidedWith.GetComponent<BubbleScript>() != null && collidedWith.GetComponent<BubbleScript>().enabled)
            {
                bubbleEnabled = true;
                collidedWith.GetComponent<BubbleScript>().pickLine();
                bubbleUI.SetActive(true);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            if (collidedWith.GetComponent<BubbleScript>() != null)
            {
                bubbleEnabled = false;
                bubbleUI.SetActive(false);
            }
            dialogue = other.GetComponentInParent<Dialogue>();
            if (dialogue != null) { dialogue.branchIndex = dialogue.startIndex; }
            inTrigger = false;
            collidedWith = null;
        }
    }

    private void Move()
    {
        handler.enabled = true;
        handler.move();
    }

}

