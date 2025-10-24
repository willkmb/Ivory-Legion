using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioTest : MonoBehaviour, Interfaces.Interfaces.IHaveSfxSounds
    {
        private AudioSource _audioSource;
        [SerializeField] private List<AudioClip> audioList;
        public string audioName;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            AddSfx(audioList); //Pass through your audio clip list
            SoundTest();
        }


        public void AddSfx(List<AudioClip> audisoClipList)
        {
            AudioManager.instance.DictionarySortingSfx(audioList); 
            //Adds audio to the SFX list (this is different per scene depending how many times you call this function on load
        }

        private void SoundTest()
        {
            if(AudioManager.instance.SfxDataBase.TryGetValue(audioName, out var clip))
            {
                _audioSource.clip = clip;
                Debug.Log(clip.name);
            }
        }
    }
}

