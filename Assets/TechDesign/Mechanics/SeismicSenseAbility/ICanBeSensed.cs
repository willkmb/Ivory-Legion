using System;
using Audio;
using UnityEngine;

public class ICanBeSensed : MonoBehaviour
{
    [Header("Parameters")]
    public SeismicSenseType senseType;
    public float minVolume;
    public float maxVolume;
    public bool alterPitch = false;
    public float minPitch;
    public float maxPitch;

    public void ReturningPulse()
    {
        
        // Audio
        string parameterValue = "";
        switch (senseType)
        {
            case SeismicSenseType.Null:
                break;
            case SeismicSenseType.Quest:
                parameterValue = "Quest";
                break;
            case SeismicSenseType.NpcWantingToTalk:
                parameterValue = "Npc Talk";
                break;
            case SeismicSenseType.QuestItem:
                parameterValue = "Quest Item";
                break;
            case SeismicSenseType.Valuable:
                parameterValue  = "Valuable";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        AudioManager.instance.PlayFMODSound(transform.position, "event:/Mechanics/Seismic Sense Hit Event", 1f,true,false,false,
            true,false, minVolume, maxVolume, 
            alterPitch, minPitch,maxPitch,
            true, 
            true, "Seismic Sense Hits", parameterValue);
    }
}
