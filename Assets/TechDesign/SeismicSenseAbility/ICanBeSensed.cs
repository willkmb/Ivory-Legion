using System;
using Audio;
using UnityEngine;

public class ICanBeSensed : MonoBehaviour
{
    public SeismicSenseType senseType;

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
        AudioManager.instance.PlayFMODSound(transform.position, "event:/Mechanics/Seismic Sense Hit Event", 1f,true,false,
            false, 0, 0, 
            true, 0.9f,1.1f,
            false, 
            true, "Seismic Sense Hits", parameterValue);
    }
}
