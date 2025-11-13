using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BubbleScript : MonoBehaviour
{
    [SerializeField] public List<Line> Lines = new List<Line>();
    private bool inTrigger;
    public GameObject bubble;
    private GameObject collidedWith;
    private GameObject MarkerLoc;
    private TextMeshProUGUI text;

    void Awake()
    {
        bubble.SetActive(false);
        text = bubble.transform.Find("Bubble/Text").GetComponent<TextMeshProUGUI>();
    }

    [System.Serializable]
    public class Line // creates a class for each line
    {
        [Header("Text Content")]
        public string textContent = "Content";
        [Header("Probability")]
        [Range(0, 10)]
        public int probability = 0;
        [Header("Play Once")]
        public bool once = false;
    }

    void Update()
    {
        GameObject elephant = GameObject.FindGameObjectWithTag("Player");
        collidedWith = elephant.GetComponent<NpcTalkTrigger>().collidedWith;
        inTrigger = elephant.GetComponent<NpcTalkTrigger>().inTrigger;
        if (inTrigger && collidedWith != null && collidedWith.GetComponent<BubbleScript>() != null)
        {
            MarkerLoc = collidedWith.transform.Find("MarkerLoc").gameObject;
            if (MarkerLoc != null) { Vector3 pos = Camera.main.WorldToScreenPoint(MarkerLoc.transform.position); bubble.transform.position = pos; }// set the speech bubble to match position of npc
        }
    }

    public void pickLine()
    {
        if (Lines.Count == 0) return;
        int total = 0;
        foreach (Line line in Lines)
        {
            total += line.probability; // add the probabilities together to get total amount
        }
        int rand = Random.Range(0, total); // get random int within the range of 0 to the max probability
        for(int i = 0; i < Lines.Count; i++)
        {
            Line line = Lines[i];
            if (rand < line.probability) // check if its lower than the probability of this line, if it is print that line
            {
                Debug.Log(line.textContent);
                text.text = line.textContent;
                if (line.once) { Lines.RemoveAt(i); } // if set to only play once, remove it from the list
                break; // only print one line
            }
            else
            {
                if (rand > line.probability)
                {
                    rand -= line.probability; // if its bigger than probability remove this probability from the total and loop back around until one line falls within range
                }
            }
        }
    }
}
