using System.Collections.Generic;
using UnityEngine;

public class Journal2Script : MonoBehaviour
{
    [SerializeField] GameObject holder;
    [SerializeField] GameObject icon;
    private List<GameObject> entries = new List<GameObject>();
    private int cap = 15;
    private int index = 0;

    private void Awake()
    {
        for(int i = 0; i < cap; i++) { GameObject cur = Instantiate(icon, holder.transform); entries.Add(cur); }
    }

    private void Update()
    {
        float wheel = Input.mouseScrollDelta.y;

        if (wheel > 0f) { index++; Debug.Log(index); }
        else if(wheel < 0f) { index--; Debug.Log(index); }
    }
}
