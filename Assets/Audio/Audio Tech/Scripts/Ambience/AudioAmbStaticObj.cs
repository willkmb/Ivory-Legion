using System.Collections.Generic;
using UnityEngine;

public class AudioAmbStaticObj : MonoBehaviour
{
    [SerializeField] private List<string> eventNames = new List<string>();
    [SerializeField] private List<Vector3> soundLocations = new List<Vector3>();
    [SerializeField] private bool reverbCheck = false;
    public Vector3 GetStaticPosition()
    {
        if (eventNames.Count > 0)
            return soundLocations[0];
        
        return soundLocations[Random.Range(0, eventNames.Count)];
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
