using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCtrustValue : MonoBehaviour
{
    public int trustValue;
    [HideInInspector] public string opinionLevel = "Neutral";
    public string[] likedTopics;
    public string[] dislikedTopics;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AmendTrustValue(string choiceTopic)
    {
        Debug.Log("Before Choice, NPC opinion:" + opinionLevel);
        foreach (string topic in likedTopics)
        {
            if (choiceTopic == topic)
            {
                trustValue += 10;
            }
        }
        foreach (string topic in dislikedTopics)
        {
            if (choiceTopic == topic)
            {
                trustValue -= 10;
            }
        }


        OpinionCheck();
    }

    public void OpinionCheck()
    {
        if (trustValue >= -5 && trustValue <= 5)
        {
            opinionLevel = "Neutral";
        }

        if (trustValue < -5)
        {
            //between -15 and 5
            if (trustValue >= -15)
            {
                opinionLevel = "Dislike";
            }
            //-15 downwards
            else
            {
                opinionLevel = "Hate";
            }
        }
        else if (trustValue > 5)
        {
            //between 5 and 15
            if (trustValue <= 15)
            {
                opinionLevel = "Like";
            }
            else
            {
                opinionLevel = "Love";
            }
        }
        text.text = "Opinion: " + opinionLevel;
    }

}
