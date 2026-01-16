using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioAmbStaticManager : MonoBehaviour
    {
        public static AudioAmbStaticManager instance;
        
        public List<AudioAmbStaticObj> audioStaticAmbSounds = new List<AudioAmbStaticObj>();

        private void Awake()
        {
            instance ??= this;
        }

        public void PlayStaticSound(float minVolume, float maxVolume)
        {
            int number;
            if (audioStaticAmbSounds.Count <= 1)
                number = 0;
            else
                number  =  Random.Range(0, audioStaticAmbSounds.Count);
            
            AudioManager.instance.PlayFMODSound(audioStaticAmbSounds[number].GetStaticPosition(), audioStaticAmbSounds[number].GetStaticEventName(), 
                2f, true, audioStaticAmbSounds[number].ReverbCheck(), true,
                true, false, minVolume, maxVolume, 
                true, 0.9f, 1.1f, 
                true, false, null, null);
        }
    } 
}

