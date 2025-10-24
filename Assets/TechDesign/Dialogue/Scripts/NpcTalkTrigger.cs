using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcTalkTrigger : MonoBehaviour
{
    TriggerScript triggerScript;
    public GameObject dialogueUI;
    Dialogue dialogue;
    Collider trigger;
    public bool inTrigger;
    private GameObject collidedWith;
    void Start()
    {
        triggerScript = GetComponentInChildren<TriggerScript>();
        dialogueUI = GameObject.Find("DialogueHolder");
        dialogueUI.gameObject.SetActive(false);
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if(collidedWith != null)
                    {
                        dialogue = collidedWith.GetComponent<Dialogue>();
                        dialogue.ShowNextBranch();
                        TextMeshProUGUI text = collidedWith.GetComponent<NPCtrustValue>().text;
                        string opinion = collidedWith.GetComponent<NPCtrustValue>().opinionLevel;
                        text.text = "Opinion: " + opinion;
                    }
                    dialogueUI.gameObject.SetActive(true);
                    dialogueUI.GetComponent<Animation>().Play();
                    Cursor.lockState = CursorLockMode.Confined;
                    //GameObject.FindGameObjectWithTag("Player").GetComponent<Move>().canMove = false;
                }
            }
            else
            {
                dialogueUI.gameObject.SetActive(false);
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
