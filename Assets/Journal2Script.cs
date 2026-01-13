using System.Collections.Generic;
using UnityEngine;

public class Journal2Script : MonoBehaviour
{
    [SerializeField] GameObject holder;
    [SerializeField] GameObject icon;
    private int cap = 15;

    private void Awake()
    {
        for(int i = 0; i < cap; i++) { Instantiate(icon, holder.transform); }
    }
}
