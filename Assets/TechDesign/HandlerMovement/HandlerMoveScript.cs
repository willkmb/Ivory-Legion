using NUnit.Framework;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class HandlerMoveScript : MonoBehaviour
{
    private int currentIndex = 0;
    private float time = 0;
    private bool moving = false;
    private bool pressed = false;
    private Vector3 startPos;

    [System.Serializable]
    public class pointClass
    {
        public GameObject point;
        public float speed = 2f;
        public float waitTime = 2f;
    }

    public List<pointClass> points = new List<pointClass>();

    private void Start()
    {
        startPos = this.transform.position;
    }

    private void Update()
    {
        if (currentIndex > points.Count -1) return;

        if (Input.GetKeyDown(KeyCode.M) && !moving && !pressed)
        {
            moving = true;
            time = 0f;
        }

        if (moving) isMoving();
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
            currentIndex++;
            Invoke("moveAgain", points[currentIndex].waitTime);
        }
    }

    public void moveAgain()
    {
        if (currentIndex > points.Count - 1) return;
        moving = true;
        time = 0f;
    }
}
