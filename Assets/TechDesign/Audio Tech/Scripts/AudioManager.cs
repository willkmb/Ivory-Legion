using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public enum SfxList
    {
        
    }
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        // When used to get audio, use the file name of the audio clip to reference it
        public Dictionary<string, AudioClip> SfxDataBase = new Dictionary<string, AudioClip>();
        public Dictionary<string, AudioClip> MusicDataBase = new Dictionary<string, AudioClip>();
        public Dictionary<string, AudioClip> AmbDataBase = new Dictionary<string, AudioClip>();
        public Dictionary<string, AudioClip> DiaDataBase = new Dictionary<string, AudioClip>();
        
        private void Awake()
        {
            instance ??= this;
        }
        
        
        public void DictionarySortingSfx(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList) 
            {
                SfxDataBase.Add(audioClip.name, audioClip); 
                //Adds all audio Clips in the list to the sfx dictionary
            }
        }
        public void DictionarySortingMusix(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList)
            {
                MusicDataBase.Add(audioClip.name, audioClip);
            }
        }
        public void DictionarySortingAmb(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList)
            {
                AmbDataBase.Add(audioClip.name, audioClip);
            }
        }
        public void DictionarySortingDia(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList)
            {
                DiaDataBase.Add(audioClip.name, audioClip);
            }
        }
    }
}
