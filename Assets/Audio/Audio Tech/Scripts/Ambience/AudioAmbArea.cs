using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Audio
{
    public class AudioAmbArea : MonoBehaviour
    {
        public string loopingAmbSound;
        public List<string> ambSoundsList = new List<string>();
        private void OnTriggerEnter(Collider other)
        {
            Interfaces.Interfaces.IPlayer player = other.transform.GetComponent<Interfaces.Interfaces.IPlayer>();
            if (player != null)
            {
                if(AudioManager.instance.soundDataBase.TryGetValue(loopingAmbSound, out var clip)) 
                {
                    AudioAmbManager.instance.ChangeAmb(clip);
                    AudioAmbManager.instance.currentAmbSoundList = ambSoundsList;
                }
            }
        }
        
    }
}

