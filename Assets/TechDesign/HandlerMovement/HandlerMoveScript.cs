using NUnit.Framework;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;
using TMPro;

public class HandlerMoveScript : MonoBehaviour
{
    private int currentIndex = 0;
    private float time = 0;
    private bool moving = false;
    private bool pressed = false;
    private Vector3 startPos;
    private bool isVisible;
    private bool waitingToMove;
    public GameObject di;
    private bool enabled = false;

    [System.Serializable]
    public class pointClass
    {
        public GameObject point;
        public float speed = 2f;
        public float waitTime = 2f;
        public bool shouldtalk;
        public string textContent;

        public int dialogueSet;
        public bool enableCut = false;
        public bool moveAgainAfter = false;
        public bool showPromptWhenReach = true;
    }

    public List<pointClass> points = new List<pointClass>();

    private void Start()
    {
        startPos = this.transform.position;
    }

    private void Update()
    {
        if (currentIndex > points.Count - 1) return;
        if (moving) isMoving();
        if (moving) this.GetComponent<PromptScript>().thisPrompt.SetActive(false); else { this.GetComponent<Dialogue>().loadSet(points[currentIndex].dialogueSet); }
    }

    public void isMoving()
    {
        time += Time.deltaTime / points[currentIndex].speed;
        this.transform.position = Vector3.Lerp(startPos, points[currentIndex].point.transform.position, time);

        Vector3 pointPos = points[currentIndex].point.transform.position;
        pointPos.y = this.transform.position.y;
        this.transform.LookAt(pointPos);

        if(time >= 1f)
        {
            moving = false;
            startPos = this.transform.position;

            if (di.activeInHierarchy == false && points[currentIndex].showPromptWhenReach) this.GetComponent<PromptScript>().thisPrompt.SetActive(true);
            if (points[currentIndex].showPromptWhenReach == false) this.GetComponent<Dialogue>().enabled = false;
            if (points[currentIndex].showPromptWhenReach) this.GetComponent<Dialogue>().enabled = true;
            if (points[currentIndex].enableCut && !enabled) { GameObject.Find("Cutscene_Parade").GetComponent<Collider>().enabled = true; enabled = true; }
            if (points[currentIndex].moveAgainAfter) moveAgain();
        }
    }

    public void moveAgain()
    {
        waitingToMove = false;
        currentIndex++;
        moving = true;
        time = 0f;
    }

    public void move()
    {
        Debug.Log("move2");
        if (currentIndex >= points.Count) return;
        if (!pressed && !moving)
        {
            Debug.Log("move");
            moving = true;
            pressed = true;
            time = 0f;
        }
        else if (!moving && pressed && !waitingToMove)
        {
            waitingToMove = true;
            Invoke("moveAgain", points[currentIndex].waitTime);
        }
    }
}