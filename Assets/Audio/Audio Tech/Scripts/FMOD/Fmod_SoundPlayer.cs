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
        public void PlaySound(string eventName, float audioLength,bool is3d,bool reverbCheck, 
            bool alterVolume, float minVolume, float maxVolume,
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
            Invoke("Reset",audioLength + audioLength * (Time.deltaTime * 0.1f));
            if (isLabelledParameter)
            {
                SoundParameterLabelled(parameterName, parameterValue, alterVolume, true, 0.85f, 1.15f, reverbMultiplier);
                return;
            }
            if (isOneShot)
            {
                OneShotSound(reverbMultiplier, alterPitch, minPitch, maxPitch);
                return;
            }
        }
        
        // E.G Seismic Sense Hits
        public void SoundParameterLabelled(string parameterName, string parameterValue, bool alterVolumeOfDist,
            bool randomPitch, float minPitch, float maxPitch, float reverbMultiplier)
        {
            currentInstance.setParameterByNameWithLabel(parameterName, parameterValue, false);
            currentInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));
            currentInstance.setReverbLevel(1, reverbMultiplier);
            
            if (randomPitch)
            {
                var randomPitchValue = Random.Range(minPitch, maxPitch);
                currentInstance.setPitch(randomPitchValue);
            }
            if (alterVolumeOfDist)
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
            else
                currentInstance.setVolume(1f);
            
            currentInstance.start();
        }

        // E.G Explosion
        private void OneShotSound(float reverbMultiplier, bool randomPitch, float minPitch, float maxPitch)
        {
            if (randomPitch)
            {
                var randomPitchValue = Random.Range(minPitch, maxPitch);
                currentInstance.setPitch(randomPitchValue);
            }
            currentInstance.setReverbLevel(1, reverbMultiplier);
            currentInstance.start();
        }
        
        // E.G Music
        private void TwoDAudio()
        {
            
        }
        // Spherecast check to see if the audio should reverberate
        private void ReverbCheck()
        {
            var playerDistToObj = Vector3.Distance(PlayerManager.instance.transform.position, currentReverbArea.highestReverbPoint.transform.position);
            if (playerDistToObj <= currentReverbArea.playerMaxDistance)
                reverbMultiplier = (currentReverbArea.reverbMultiplier * (currentReverbArea.playerMaxDistance / playerDistToObj)) / 10;
        }
        // Resets the audio player to be used again
        public void Reset()
        {
            if (rgb != null)
                Destroy(rgb);
            
            boxCollider.enabled = false;
            currentInstance.release();
            gameObject.SetActive(false);
            AudioManager.instance.audioPoolFreeFMOD.Add(gameObject);
        }
    }
}

