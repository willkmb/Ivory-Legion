using System.Collections.Generic;
using Audio.FMOD;
using UnityEngine;

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
        public void PlayFMODSound(Vector3 spawnPosition,string eventName, float audioLength,bool is3d,bool reverbCheck,
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

            fmodSoundPlayer.PlaySound(eventName, audioLength, is3d, reverbCheck, 
                alterVolumeOfDist,alterVolume, minVolume, maxVolume, 
                alterPitch, minPitch, maxPitch, 
                isOneShot, 
                isLabelledParameter, parameterName, parameterValue);
        }

// Old Code
////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ReSharper disable Unity.PerformanceAnalysis
        /// Call when you want to use a sound
    /*public void PlayAudio(string audioName,Vector3 spawnPosition,bool loops, bool is3d, bool ambSound,
            float minVolume, float maxVolume, bool randomPitch, float minPitch, float maxPitch, int priority)
        {
            if (!soundDataBase.TryGetValue(audioName, out AudioClip audioClip))
            {
                Debug.LogError(audioName + " - Doesn't exist");
                return;
            }
            
            GameObject audioObj = audioPoolFree[0];
            audioPoolFree.Remove(audioObj);
            audioObj.transform.position = spawnPosition;
            audioObj.SetActive(true);
            AudioSource audioSource = audioObj.transform.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                if (loops)
                {
                    audioSource.loop = true;
                   // AudioAmbManager.instance.loopingAudioPlayersList.Add(audioSource);
                }
                audioSource.spatialBlend = is3d ? 1 : 0;
            }
            
            //Used later to disable the obj after x seconds, then re adds it to the free objPool
            AudioPlayer audioPlayer = audioObj.GetComponent<AudioPlayer>();
            
            //Gets the audio source and sets it at a random volume, if the volume never randomises set both min/max to the same value
            audioSource.clip = soundDataBase[audioName];
            var newVolume = Random.Range(minVolume, maxVolume);
            audioSource.volume = newVolume;
            
            audioSource.priority = priority; //Default 128, this means if it is heard over other sounds with similar volume levels
            
            //Random Pitch if chooses to do so
            if (randomPitch)
                audioSource.pitch = Random.Range(minPitch, maxPitch);
           
            // Plays the audio
            audioSource.Play();
            
            // if(ambSound)
            //      AudioAmbManager.instance.FoliageSoundChecker(audioName, audioSource.clip.length);
            //
            //Disables audio after x seconds, adding it back to the audioPoolFree list
            if (loops)
                return;
            audioPlayer.Invoke("DisableObj", audioSource.clip.length + (delayTime - Random.Range(delayTime * 0.1f, delayTime * 0.35f)));
            
            
            
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Object Pooling Functions
        private void SpawnObjectPool()
        {
            for (int i = 0; i < 100; i++)
            {
                GameObject instantiate = Instantiate(audioPrefab, transform.position, Quaternion.identity);
                instantiate.transform.SetParent(transform);
                audioPoolFree.Add(instantiate);
                instantiate.SetActive(false);
            }
        }
        
               private void Start()
        {
            SpawnObjectPoolFMOD();
            
            // DictionarySortingSound(sfxList);
            // DictionarySortingMusic(musicList);
            // DictionarySortingAmb(ambList);
            // DictionarySortingDialogue(diaList);
            //
            // DictionarySortingCatalystAmb(ambCatalystList);
        }
        /*private void DictionarySortingSound(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList) 
            {
                soundDataBase.Add(audioClip.name, audioClip); 
                //Adds all audio Clips in the list to the sound dictionary
            }
        }
        private void DictionarySortingMusic(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList) 
            {
                soundDataBase.Add(audioClip.name, audioClip); 
                //Adds all audio Clips in the list to the sound dictionary
            }
        }
        private void DictionarySortingAmb(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList) 
            {
                soundDataBase.Add(audioClip.name, audioClip); 
                //Adds all audio Clips in the list to the sound dictionary
            }
        }
        private void DictionarySortingDialogue(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList) 
            {
                soundDataBase.Add(audioClip.name, audioClip); 
                //Adds all audio Clips in the list to the sound dictionary
            }
        }
        private void DictionarySortingCatalystAmb(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList) 
            {
                catalystAmbAudio.Add(audioClip.name, audioClip); 
                //Adds all audio Clips in the list to the sound dictionary
            }
        }* /

        }*/
    }
}
