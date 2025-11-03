using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Npc.AI;
using Player.Dialogue;
using Player;
using InputManager;

public class Dialogue : MonoBehaviour
{
    public List<DialogueSet> DialogueSets = new List<DialogueSet>(); // creates a list of the dialogues to create different interactions

    [Header("Dialogue Settings")]
    [HideInInspector] public int startIndex = 0;
    [HideInInspector] public int branchIndex;
    private int DialogueStage = 0;

    [Header("Text Assign")]
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI dialogueText;

    [Header("Box Assign")]
    public GameObject dialogueBox;
    public GameObject choiceRoot;
    public GameObject choiceButton;

    [Header("Cursor Assign")]
    public GameObject cursorRoot;
    public GameObject cursor;

    [Header("Image Assign")]
    public GameObject charImage;

    NPCtrustValue trustHolder;
    private NpcManager _npcManager;
    [HideInInspector] public NpcState pastNpcState;

    [System.Serializable]
    public class dialogue // same as above but for main body text
    {
        [Header("Speaker Name")]
        public string name;
        [Header("Text content for dialogue")]
        public string dialogueTextContent;
        [Header("Change the character image")]
        public Sprite image;

        [System.Serializable]
        public class dialogueChoice //creates a class with option and branch index, System.serializable exposes it to the inspector for easy adaptability and modularity
        {
            [Header("Text content for choices")]
            public string choiceTextContent;
            [Header("Trust value change")]
            public string choiceTopic;
            [Header("Set the index to the next branch the choice leads to")]
            public int nextBranch;
            [Header("Which branch this choice belongs to")]
            public int rootBranch;
        }
        public List<dialogueChoice> choices = new List<dialogueChoice>();
    }

    [System.Serializable]
    public class DialogueSet
    {
        public List<dialogue> Dialogues = new List<dialogue>(); // Creates a list in the inspector of the class system to create as many options as we want without hard coding it
    }

    private void Start()
    {
        trustHolder = GetComponent<NPCtrustValue>();
        branchIndex = startIndex; // sets the current branch index to the start
        ShowNextBranch();
    }

    public void loadSet()
    {
        if (DialogueSets.Count == 0) return;
        DialogueStage++;
        DialogueStage = Mathf.Clamp(DialogueStage, 0, DialogueSets.Count);
        branchIndex = startIndex;
    }

    public void ShowNextBranch()
    {
        if (trustHolder == null) trustHolder = GetComponent<NPCtrustValue>();
        if (DialogueStage == DialogueSets.Count) DialogueStage -= 1;
        if (branchIndex < DialogueSets[DialogueStage].Dialogues.Count) // checks if we are within the bounds of how many dialogue options we have
        {
            dialogueText.text = DialogueSets[DialogueStage].Dialogues[branchIndex].dialogueTextContent; // sets dialogue text to the field defined in dialogues class
            speakerName.text = DialogueSets[DialogueStage].Dialogues[branchIndex].name; //  sets the speakers name to the field defined in dialogues class
            charImage.GetComponent<Image>().sprite = DialogueSets[DialogueStage].Dialogues[branchIndex].image; // change the characters image

            foreach (Transform child in choiceRoot.transform) // destroy previous choices
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in cursorRoot.transform) // destroy arrow
            {
                Destroy(child.gameObject);
            }

            foreach (var choice in DialogueSets[DialogueStage].Dialogues[branchIndex].choices) // loops through each choice option we've set
            {
                if (choice.rootBranch == branchIndex) // if the starting branch index equals the current branch index, then we create a new set of choices buttons
                {
                    GameObject Button = Instantiate(choiceButton, choiceRoot.transform);
                    GameObject buttonTextObj = Button.transform.Find("ChoiceButtonText")?.gameObject; // gets the text component in the child of button and sets it to the choice text field defined in choices
                    buttonTextObj.GetComponent<TextMeshProUGUI>().text = choice.choiceTextContent;

                    int next = choice.nextBranch;
                    Button.GetComponent<Button>().onClick.AddListener(() => { branchIndex = next; UpdateNPCOpinion(choice.choiceTopic); ShowNextBranch(); }); // adds the functionality of the button withought having to manually do it in inspector and calls advance branch method.
                }
            }

            GameObject cursorObj = null;
            if (branchIndex == DialogueSets[DialogueStage].Dialogues.Count - 1) cursorObj = Instantiate(cursor, cursorRoot.transform); //if on the last dialogue in the list, spawn arrow that closes dialogue
            if (cursorObj != null) { cursorObj.GetComponent<Button>()?.onClick.AddListener(() => { HideDialogue(); }); return; } // set the action of the button to close dialogue 

            if (DialogueSets[DialogueStage].Dialogues[branchIndex].choices.Count < 1) cursorObj = Instantiate(cursor, cursorRoot.transform); // if there is no choices, spawn the arrow
            if (cursorObj != null) cursorObj.GetComponent<Button>()?.onClick.AddListener(() => { branchIndex++; ShowNextBranch(); }); // set the action of the button to go to the next branch

            if (branchIndex >= DialogueSets[DialogueStage].Dialogues.Count)
            {
                // NPC state changers
                _npcManager.npcState = pastNpcState;
                if (_npcManager.npcState == NpcState.SetPathingWalking)
                    _npcManager.setPathWalking.currentPointNumber -= 1;
                _npcManager.StateChanger();
            }
        }
    }

    public void HideDialogue()
    {
        GameObject dialogueUI = GameObject.Find("DialogueHolder");
        dialogueUI.SetActive(false);
        branchIndex = startIndex;
        Cursor.lockState = CursorLockMode.None;
        loadSet(); // call this whenever we want it to change to the next set of dialogue
        ShowNextBranch();
        ControllerCursor cursor = GameObject.Find("ContCursor").GetComponent<ControllerCursor>();
        cursor.CursorState(false);
    }

    public void UpdateNPCOpinion(string topic)
    {
        trustHolder.AmendTrustValue(topic);
    }
}

//made by will, age 21, the train type !DO NOT STEAL, THIS WILL BE ENFORCED WITH THE DEATH PENALTY!
