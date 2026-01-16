using System;
using UnityEngine;

namespace Audio
{
    public class TestingAudioScript : MonoBehaviour
    {
        [Header("Timers")] 
        [SerializeField] private bool useTimer = false;
        [SerializeField] private float currentTime;
        [SerializeField] private float frequency;
        [Header("Base Parameters")]
        [SerializeField] private string eventName;
        [SerializeField] private float audioLength;
        [SerializeField] private bool is3d;
        [SerializeField] private bool reverbCheck;
        [SerializeField] private bool isAmb;
        [Header("Volume Parameters")]
        [SerializeField] private bool alterVolumeOfDist;
        [SerializeField] private bool alterVolume;
        [SerializeField] private float minVolume;
        [SerializeField] private float maxVolume;
        [Header("Pitch Parameters")]
        [SerializeField] private bool alterPitch;
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;
        [Header("FMOD Specific Parameters")]
        [SerializeField] private bool isOneShot;
        [SerializeField] private bool isLabelledParameter;
        [SerializeField] private string parameterName;
        [SerializeField] private string parameterValue;
        
        private void Update()
        {
            if (!useTimer)
                return;
            
            currentTime += Time.deltaTime;
            if (currentTime > frequency)
            {
                currentTime = 0;
                AudioManager.instance.PlayFMODSound(transform.position, eventName, audioLength, is3d, reverbCheck, isAmb, 
                    alterVolumeOfDist, alterVolume, minVolume, maxVolume, 
                    alterPitch, minPitch, maxPitch, 
                    isOneShot, isLabelledParameter, parameterName, parameterValue);
            }
        }
    }
}

