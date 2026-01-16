using System;
using System.Collections;
using System.Collections.Generic;
using Audio.FMOD;
using FMOD.Studio;
using FMODUnity;
using InputManager;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Audio
{
    public class ReverbArea : MonoBehaviour
    {
        [Header("Values")]
        // public GameObject highestReverbPoint;
        // [Range(0,1)] public float reverbMultiplier;
        // [Range(0,50)] public float playerMaxDistance;
        [Header("List of Players in Area")]
        [SerializeField] private List<Fmod_SoundPlayer> playersList;
        [Header("Reverb Snapshot")]
        [SerializeField] private string snapshotName;
        private EventInstance snapShotInstance;
        private void OnTriggerEnter(Collider other)
        {
            PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
            if (playerManager != null && !onCooldown)
            {
                onCooldown = true;
                EnableSnapshot();
                StartCoroutine(SnapshotCoolDown(0.25f));
                return;
            }
            Fmod_SoundPlayer player = other.gameObject.transform.GetComponent<Fmod_SoundPlayer>();
            if (other != null)
            {
                player.currentReverbArea = this;
                playersList.Add(other.gameObject.GetComponent<Fmod_SoundPlayer>());
            }
        }
        private void OnTriggerExit(Collider other)
        {
            PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
            if (playerManager != null)
                DisableSnapshot();
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void EnableSnapshot()
        {
            snapShotInstance = RuntimeManager.CreateInstance(snapshotName);
            snapShotInstance.start();
        }
        
        private void DisableSnapshot()
        {
            snapShotInstance.stop(STOP_MODE.IMMEDIATE);
            snapShotInstance.release();
        }

        private bool onCooldown;
        IEnumerator SnapshotCoolDown(float secs)
        {
            yield return new WaitForSeconds(secs);
            onCooldown = false;
        }
    }
}

