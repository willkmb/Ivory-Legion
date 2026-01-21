using System.Collections.Generic;
using UnityEngine;

public class AudioAmbStaticObj : MonoBehaviour
{
    [SerializeField] private List<string> eventNames = new List<string>();
    [SerializeField] private bool reverbCheck = false;
    public Vector3 GetStaticPosition()
    {
        return transform.position;
    }

    public string GetStaticEventName()
    {
        if (eventNames.Count > 0)
            return eventNames[0];
        
        return eventNames[Random.Range(0, eventNames.Count)];
    }

    public bool ReverbCheck()
    {
        return reverbCheck;
    }
}
