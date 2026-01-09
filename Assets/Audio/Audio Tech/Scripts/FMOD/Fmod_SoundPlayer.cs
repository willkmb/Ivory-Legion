using System.Collections;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using InputManager;
using SeismicSense;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace Audio.FMOD
{
    public class Fmod_SoundPlayer : MonoBehaviour
    {
        [Header("Layer Masks")]
        [SerializeField] private LayerMask reverbLayerMask;
        [SerializeField] private LayerMask excludeColliderLayerMask;
        
        [Header("Components")]
        [SerializeField] private BoxCollider boxCollider;
        private Rigidbody rgb;
        private EventInstance currentInstance;
        private float reverbMultiplier;
        public ReverbArea currentReverbArea;
        
       // Checks what type of audio it is
       //Checks if the sounds requires reverb
        public void PlaySound(string eventName, float audioLength, bool is3d, bool reverbCheck, 
            bool alterVolumeOfDist, bool alterVolume, float minVolume, float maxVolume,
            bool alterPitch, float minPitch, float maxPitch,
            bool isOneShot,
            bool isLabelledParameter, string parameterName, string parameterValue)
        {
            currentInstance = RuntimeManager.CreateInstance(eventName);
            if (reverbCheck)
            {
                boxCollider.enabled = true;
                rgb = transform.AddComponent<Rigidbody>();
                rgb.useGravity = false;
                rgb.freezeRotation = true;
                Invoke("ReverbCheck",Time.deltaTime * 2);
            }
            StartCoroutine(ResetAudio(audioLength + audioLength * (Time.deltaTime * 0.1f)));
            if (isLabelledParameter)
            {
                SoundParameterLabelled(parameterName, parameterValue, alterVolumeOfDist, alterVolume, minVolume, maxVolume, alterPitch, minPitch, maxPitch);
                return;
            }
            if (isOneShot)
            {
                OneShotSound(alterPitch, minPitch, maxPitch, alterVolumeOfDist);
                return;
            }

            if (!is3d)
            {
                TwoDAudio(alterVolume, minVolume, maxVolume, alterPitch, minPitch, maxPitch);
                return;
            }
        }
        
        // E.G Seismic Sense Hits
        private void SoundParameterLabelled(string parameterName, string parameterValue,
            bool alterVolumeOfDist, bool alterVolume, float minVolume, float maxVolume,
            bool randomPitch, float minPitch, float maxPitch)
        {
            currentInstance.setParameterByNameWithLabel(parameterName, parameterValue, false);
            currentInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));
            
            if (randomPitch)
                AlterPitch(minPitch, maxPitch);
            if (alterVolumeOfDist) AlterVolumeOfDistance(); else currentInstance.setVolume(1f);
            if (alterVolume)
                AlterVolume(minVolume, maxVolume);
            
            currentInstance.start();
        }

        // E.G Explosion
        private void OneShotSound(bool randomPitch, float minPitch, float maxPitch, bool alterVolumeOfDist)
        {
            if (randomPitch)
                AlterPitch(minPitch, maxPitch);
            if (alterVolumeOfDist) AlterVolumeOfDistance(); else currentInstance.setVolume(1f);
            
            currentInstance.start();
        }
        
        // E.G Music
        private void TwoDAudio(bool alterVolume, float minVolume, float maxVolume, bool alterPitch,  float minPitch, float maxPitch)
        {
            if (alterVolume)
                AlterVolume(minVolume, maxVolume);
            if (alterPitch)
                AlterPitch(minPitch, maxPitch);
            
            currentInstance.start();
        }
        // Reverb is an echo effect
        private void ReverbCheck()
        {
            if (currentReverbArea == null)
                return;
            
            var playerDistToObj = Vector3.Distance(PlayerManager.instance.transform.position, currentReverbArea.highestReverbPoint.transform.position);
            if (playerDistToObj <= currentReverbArea.playerMaxDistance)
                reverbMultiplier = (currentReverbArea.reverbMultiplier * (currentReverbArea.playerMaxDistance / playerDistToObj)) / 10;
            
            currentInstance.setReverbLevel(1, reverbMultiplier);
        }
        private void AlterVolumeOfDistance()
        {
            var dist = Vector3.Distance(transform.position, PlayerManager.instance.transform.position);
            var percentage = dist / SeismicSenseScript.instance.rangeMax;
            if (percentage <= 0.15f)
                currentInstance.setVolume(0.75f);
            if (percentage < 0.15f && percentage > 0.85f)
                currentInstance.setVolume(0.45f);
            if (percentage >= 0.85f)
                currentInstance.setVolume(0.25f);
        }
        private void AlterVolume(float minVolume, float maxVolume)
        {
            currentInstance.setVolume(Random.Range(minVolume, maxVolume));
        }

        private void AlterPitch(float minPitch, float maxPitch)
        {
            currentInstance.setPitch(Random.Range(minPitch, maxPitch));
        }
        // Resets the audio player to be used again
        IEnumerator ResetAudio(float secs)
        {
            yield return new WaitForSeconds(secs);
            
            if (rgb)
                Destroy(rgb);
            
            boxCollider.enabled = false;
            currentInstance.release();
            gameObject.SetActive(false);
            AudioManager.instance.audioPoolFreeFMOD.Add(gameObject);
        }
    }
}

