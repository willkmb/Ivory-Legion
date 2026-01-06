using System;
using System.Collections.Generic;
using Audio.FMOD;
using UnityEngine;

namespace Audio
{
    public class ReverbArea : MonoBehaviour
    {
        [Header("Values")]
        public GameObject highestReverbPoint;
        [Range(0,1)] public float reverbMultiplier;
        [Range(0,50)] public float playerMaxDistance;
        [Header("List of Players in Area")]
        [SerializeField] private List<Fmod_SoundPlayer> playersList;
        private void Start()
        {
            Invoke("Test",1f);
        }

        private void Test()
        {
            AudioManager.instance.PlayFMODSound(transform.position, "event:/SFX/Walking/WalkingTest", 1f,true, true,false, 1, 1, true, false, null, null);
        }

        private void OnTriggerEnter(Collider other)
        {
            Fmod_SoundPlayer player = other.gameObject.transform.GetComponent<Fmod_SoundPlayer>();
            player.currentReverbArea = this;
            playersList.Add(other.gameObject.GetComponent<Fmod_SoundPlayer>());
        }
    }
}

