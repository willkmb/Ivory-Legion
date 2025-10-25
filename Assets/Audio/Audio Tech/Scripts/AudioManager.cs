using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("Object Pool Obj")]
         public List<GameObject> audioPoolFree;
         public List<GameObject> audioPoolLocked;
        [SerializeField] private GameObject audioPrefab;
        
        [Header("Audio Lists of Entire Game")]
        private AudioSource _audioSource;
        [SerializeField] private List<AudioClip> sfxList;
        [SerializeField] private List<AudioClip> musicList;
        [SerializeField] private List<AudioClip> ambList;
        [SerializeField] private List<AudioClip> diaList;

        // When used to get audio, use the file name of the audio clip to reference it
        public Dictionary<string, AudioClip> SfxDataBase = new Dictionary<string, AudioClip>();
        public Dictionary<string, AudioClip> MusicDataBase = new Dictionary<string, AudioClip>();
        public Dictionary<string, AudioClip> AmbDataBase = new Dictionary<string, AudioClip>();
        public Dictionary<string, AudioClip> DiaDataBase = new Dictionary<string, AudioClip>();
        
        private void Awake()
        {
            instance ??= this;
            
            _audioSource  = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            
            audioPoolFree = new List<GameObject>();
            audioPoolLocked = new List<GameObject>();
        }
        private void Start()
        {
            SpawnObjectPool();
            
            DictionarySortingSfx(sfxList);
            DictionarySortingMusic(musicList);
            DictionarySortingAmb(ambList);
            DictionarySortingDia(diaList);
        }

        private void DictionarySortingSfx(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList) 
            {
                SfxDataBase.Add(audioClip.name, audioClip); 
                //Adds all audio Clips in the list to the sfx dictionary
            }
        }
        private void DictionarySortingMusic(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList)
            {
                MusicDataBase.Add(audioClip.name, audioClip);
            }
        }
        private void DictionarySortingAmb(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList)
            {
                AmbDataBase.Add(audioClip.name, audioClip);
            }
        }
        private void DictionarySortingDia(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList)
            {
                DiaDataBase.Add(audioClip.name, audioClip);
            }
        }
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Amb changer
        public void ChangeAmb(AudioClip audioName)
        {
            _audioSource.clip = audioName;
        }
/// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        // Call when you want to use a sfx sound, E.G Elephant movement
        public void PlaySfxAudio(string audioName,Vector3 spawnPosition, float minVolume, float maxVolume, bool randomPitch, float minPitch, float maxPitch, int priority)
        {
            GameObject audioObj = audioPoolFree[0];
            audioPoolFree.Remove(audioObj);
            audioObj.transform.position = spawnPosition;
            audioObj.SetActive(true);
            AudioSource audioSource = audioObj.transform.GetComponent<AudioSource>();
            
            //Used later to disable the obj after x seconds, then re adds it to the free objPool
            AudioDisablePoolObj audioDisablePoolObj = audioObj.GetComponent<AudioDisablePoolObj>();
            
            //Gets the audio source and sets it at a random volume, if the volume never randomises set both min/max to the same value
            audioSource.clip = SfxDataBase[audioName];
            var newVolume = Random.Range(minVolume, maxVolume);
            audioSource.volume = newVolume;
            
            audioSource.priority = priority; //Default 128, this means if it is heard over other sounds with similar volume levels
            
            //Random Pitch if chooses to do so
            if (randomPitch)
                audioSource.pitch = Random.Range(minPitch, maxPitch);
           
            // Plays the audio
            audioSource.Play();
            //Disables audio after x seconds, adding it back to the audioPoolFree list
            audioDisablePoolObj.Invoke("DisableObj", audioSource.clip.length);
        }
    }
}
