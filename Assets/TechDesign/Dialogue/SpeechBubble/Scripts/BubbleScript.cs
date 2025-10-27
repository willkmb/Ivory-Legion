using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BubbleScript : MonoBehaviour
{
    [SerializeField] public List<Line> Lines = new List<Line>();
    private bool inTrigger;
    private GameObject bubble;
    private GameObject collidedWith;
    private GameObject MarkerLoc;
    private TextMeshProUGUI text;

    void Awake()
    {
        bubble = GameObject.Find("SpeechBubbleHolder");
        bubble.SetActive(false);
        text = bubble.transform.Find("Bubble/Text").GetComponent<TextMeshProUGUI>();
    }

    [System.Serializable]
    public class Line
    {
        [Header("Text Content")]
        public string textContent = "Content";
        [Header("Probability")]
        public int probability = 0;
    }

    void Update()
    {
        if(inTrigger && collidedWith != null)
        {
            Renderer rend = collidedWith.GetComponent<Renderer>();
            bool isVisible = rend.isVisible;
            if (isVisible)
            {
                MarkerLoc = collidedWith.transform.Find("MarkerLoc").gameObject;
                Vector3 pos = Camera.main.WorldToScreenPoint(MarkerLoc.transform.position);
                bubble.transform.position = pos;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            inTrigger = true;
            collidedWith = other.gameObject;
            bubble.SetActive(true);

            if (Lines.Count == 0) return;
            int total = 0;
            foreach (Line line in Lines)
            {
                total += line.probability;
            }
            int rand = Random.Range(0, total);
            foreach (Line line in Lines)
            {
                if (rand < line.probability)
                {
                    Debug.Log(line.textContent);
                    text.text = line.textContent;
                    break;
                }
                else
                {
                    if (rand > line.probability)
                    {
                        rand -= line.probability;
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            inTrigger = false;
            bubble.SetActive(false);
            collidedWith = null;
        }
    }
}
