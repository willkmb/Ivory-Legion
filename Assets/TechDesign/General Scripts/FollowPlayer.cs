using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    GameObject playerPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerPoint = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = new Vector3(playerPoint.transform.position.x, 0, 0);
        gameObject.transform.position = playerPoint.transform.position;
    }
}

