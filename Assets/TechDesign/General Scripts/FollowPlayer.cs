using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static FollowPlayer instance;
    [HideInInspector] public bool canFollow;
    [SerializeField] private float yMinusLevel;
    
    GameObject playerPoint;

    private void Awake()
    {
        instance ??= this;
        canFollow = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerPoint = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = new Vector3(playerPoint.transform.position.x, 0, 0);
        if (canFollow)
        {
            Vector3 var = new Vector3(playerPoint.transform.position.x - yMinusLevel, transform.position.y, playerPoint.transform.position.z);
            gameObject.transform.position = var;
        }
            
    }
}

