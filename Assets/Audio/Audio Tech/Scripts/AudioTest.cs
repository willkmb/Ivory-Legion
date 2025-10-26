using System.Collections.Generic;
using UnityEngine;

namespace Audio // DO NOT change this name
{
    public class AudioTest : MonoBehaviour // <- Interface to acquire void "AddSfx"
    {
        //IMPORTANT NOTES
        //If using more than one audio source, keep track of which ones are getting audio clips added to them
        //When adding audio, variation is great in pitch and volume
        //String to fetch the audio has to be spelt the EXACT same way otherwise it will not work
        //When your list is added to the main Dictionary, only add your sounds once, it doesn't like duplicates.
        //To help with the above, a good rule is only putting the list of sounds on a manager / singleton script
        
        private AudioSource _audioSource; //Where your sound is set too
        public string audioName; //Most likely going to be public as scripts will change this
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            SoundTest(); // Purely here for showcase
        }
        
        private void SoundTest()
        {
            // Gets the name of the audio clip requested and outputs that clip into "out var clip"
            if(AudioManager.instance.soundDataBase.TryGetValue(audioName, out var clip)) 
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
        }
    }
}

