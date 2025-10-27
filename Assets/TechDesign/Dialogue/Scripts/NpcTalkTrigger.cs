using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcTalkTrigger : MonoBehaviour
{
    TriggerScript triggerScript;
    public GameObject dialogueUI;
    public GameObject bubbleUI;
    Dialogue dialogue;
    Collider trigger;
    public bool inTrigger;
    [HideInInspector] public GameObject collidedWith;

    private bool bubbleEnabled = false;
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
                if (Input.GetKeyDown(KeyCode.E) && !bubbleEnabled)
                {
                    if(collidedWith != null)
                    {
                        dialogue = collidedWith.GetComponent<Dialogue>();
                        dialogue.ShowNextBranch();
                        TextMeshProUGUI text = collidedWith.GetComponent<NPCtrustValue>().text;
                        string opinion = collidedWith.GetComponent<NPCtrustValue>().opinionLevel;
                        text.text = "Opinion: " + opinion;
                    }
                    dialogueUI.SetActive(true);
                    dialogueUI.GetComponent<Animation>().Play();
                    Cursor.lockState = CursorLockMode.Confined;
                }
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
            if (collidedWith.GetComponent<BubbleScript>() != null && collidedWith.GetComponent<BubbleScript>().enabled)
            {
                bubbleEnabled = true;
                collidedWith.GetComponent<BubbleScript>().pickLine();
                bubbleUI.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            inTrigger = false;
            dialogue = collidedWith.GetComponent<Dialogue>();
            dialogue.branchIndex = dialogue.startIndex;
            if(collidedWith.GetComponent<BubbleScript>() != null)
            {
                bubbleEnabled = false;
                bubbleUI.SetActive(false);
            }
            collidedWith = null;
        }
    }

}
