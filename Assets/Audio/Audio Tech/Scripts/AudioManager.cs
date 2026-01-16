using System.Collections.Generic;
using Audio.FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("Variables")]
        [Range(0.5f, 25f)] [SerializeField] private float delayTime;
        
        // FMOD Stuff
        public List<GameObject> audioPoolFreeFMOD;
        [SerializeField] private GameObject fmodAudioPrefab;
        
        private void Awake()
        {
            instance ??= this;
            
            DontDestroyOnLoad(gameObject);
            
            SpawnObjectPoolFMOD();
        }
// FMOD Object Pooling Functions
        private void SpawnObjectPoolFMOD()
        {
            for (int i = 0; i < 100; i++)
            {
                GameObject instantiate = Instantiate(fmodAudioPrefab, transform.position, Quaternion.identity);
                instantiate.transform.SetParent(transform);
                audioPoolFreeFMOD.Add(instantiate);
                instantiate.SetActive(false);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ReSharper disable Unity.PerformanceAnalysis
        /// FMOD
        public void PlayFMODSound(Vector3 spawnPosition,string eventName, float audioLength,bool is3d,bool reverbCheck, bool isAmb,
              bool alterVolumeOfDist,bool alterVolume, float minVolume, float maxVolume, 
              bool alterPitch, float minPitch, float maxPitch,
              bool isOneShot,
              bool isLabelledParameter, string parameterName, string parameterValue)
        {
            GameObject audioObj = audioPoolFreeFMOD[Random.Range(0, audioPoolFreeFMOD.Count -1)];
            audioPoolFreeFMOD.Remove(audioObj);
            audioObj.transform.position = spawnPosition;
            Fmod_SoundPlayer fmodSoundPlayer = audioObj.GetComponent<Fmod_SoundPlayer>();
            if (fmodSoundPlayer == null)
            {
                Debug.LogError("No FMOD sound player found");
                return;
            }
            audioObj.SetActive(true);

            fmodSoundPlayer.PlaySound(eventName, audioLength, is3d, reverbCheck, isAmb,
                alterVolumeOfDist,alterVolume, minVolume, maxVolume, 
                alterPitch, minPitch, maxPitch, 
                isOneShot, 
                isLabelledParameter, parameterName, parameterValue);
        }
    }
}
