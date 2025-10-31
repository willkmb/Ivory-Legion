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
    public GameObject bubble;
    public TextMeshProUGUI text;
    public GameObject MarkerLoc;
    private bool waitingToMove;

    [System.Serializable]
    public class pointClass
    {
        public GameObject point;
        public float speed = 2f;
        public float waitTime = 2f;
        public bool shouldtalk;
        public string textContent;
    }

    public List<pointClass> points = new List<pointClass>();

    private void Start()
    {
        startPos = this.transform.position;
    }

    private void Update()
    {
        if (currentIndex > points.Count -1) return;
        if (moving) isMoving();

        if (points[currentIndex].shouldtalk && !moving)
        {
            if (isVisible)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(MarkerLoc.transform.position);
                bubble.transform.position = pos;
            }
        }

        if (moving)
        {
            this.transform.Find("TriggerHandler").gameObject.SetActive(false);
        }
        else
        {
            this.transform.Find("TriggerHandler").gameObject.SetActive(true);
        }
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
        }
    }

    public void moveAgain()
    {
        waitingToMove = false;
        currentIndex++;
        moving = true;
        time = 0f;
        isVisible = false;
        bubble.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !moving)
        {
            if (currentIndex >= points.Count) return;
            isVisible = true;
            if (!pressed && !moving)
            {
                moving = true;
                pressed = true;
                time = 0f;
            }
            else if (!moving && pressed && !waitingToMove)
            {
                waitingToMove = true;
                Invoke("moveAgain", points[currentIndex].waitTime);
                if (points[currentIndex].shouldtalk)
                {
                    bubble.SetActive(true);
                    text.text = points[currentIndex].textContent;
                }
            }
        }
    }
}