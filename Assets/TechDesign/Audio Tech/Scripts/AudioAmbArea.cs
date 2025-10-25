using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Audio
{
    public class AudioAmbArea : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> audioList;
        public string audioName;
        private void OnTriggerEnter(Collider other)
        {
            Interfaces.Interfaces.IPlayer player = other.transform.GetComponent<Interfaces.Interfaces.IPlayer>();
            if (player != null)
            {
                if(AudioManager.instance.SfxDataBase.TryGetValue(audioName, out var clip)) 
                {
                    AudioManager.instance.ChangeAmb(clip);
                }
            }
        }
    }
}

