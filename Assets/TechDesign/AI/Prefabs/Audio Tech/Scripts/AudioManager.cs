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
        
        private void Awake()
        {
            instance ??= this;
        }
        

        public void DictionarySortingSfx(List<AudioClip> audioList)
        {
            foreach(var audioClip in audioList)
            {
                SfxDataBase.Add(audioClip.name, audioClip);
            }
        }
    }
}
