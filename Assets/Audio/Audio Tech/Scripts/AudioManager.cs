using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("Variables")]
        [Range(0.5f, 25f)] [SerializeField] private float delayTime;
        
        [Header("Object Pool Obj")]
         public List<GameObject> audioPoolFree;
        [SerializeField] private GameObject audioPrefab;
        
        [Header("Audio Lists of Entire Game")]
        [SerializeField] private List<AudioClip> sfxList;
        [SerializeField] private List<AudioClip> musicList;
        [SerializeField] private List<AudioClip> ambList;
        [SerializeField] private List<AudioClip> diaList;
        
        [Header("Audio Lists for Catalyst Audio")]
        [SerializeField] private List<AudioClip> ambCatalystList;

        // When used to get audio, use the file name of the audio clip to reference it
        public Dictionary<string, AudioClip> soundDataBase = new Dictionary<string, AudioClip>();
        
        public Dictionary<string, AudioClip> catalystAmbAudio = new Dictionary<string, AudioClip>();
        
        private void Awake()
        {
            instance ??= this;
            
            DontDestroyOnLoad(gameObject);
            
            audioPoolFree = new List<GameObject>();
        }
        private void Start()
        {
            SpawnObjectPool();
            
            DictionarySortingSound(sfxList);
            DictionarySortingMusic(musicList);
            DictionarySortingAmb(ambList);
            DictionarySortingDialogue(diaList);

            DictionarySortingCatalystAmb(ambCatalystList);
        }

        private void DictionarySortingSound(List<AudioClip> audioList)
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
        }
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
////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ReSharper disable Unity.PerformanceAnalysis
        /// Call when you want to use a sound
        public void PlayAudio(string audioName,Vector3 spawnPosition,bool loops, bool is3d, bool ambSound,
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
                    AudioAmbManager.instance.loopingAudioPlayersList.Add(audioSource);
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
            
            if(ambSound)
                 AudioAmbManager.instance.FoliageSoundChecker(audioName, audioSource.clip.length);
            
            //Disables audio after x seconds, adding it back to the audioPoolFree list
            if (loops)
                return;
            audioPlayer.Invoke("DisableObj", audioSource.clip.length + (delayTime - Random.Range(delayTime * 0.1f, delayTime * 0.35f)));
        }
    }
}
