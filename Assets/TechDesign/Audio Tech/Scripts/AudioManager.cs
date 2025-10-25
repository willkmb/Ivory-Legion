using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        
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
        }

        private void Start()
        {
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
        
        public void ChangeAmb(AudioClip audioName)
        {
            _audioSource.clip = audioName;
        }
    }
}
