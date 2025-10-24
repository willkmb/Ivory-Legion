using System.Collections.Generic;
using UnityEngine;

namespace Prefabs.Audio.Scripts
{
    public enum SfxList
    {
        
    }
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public Dictionary<AudioClip, string> SfxDataBase = new Dictionary<AudioClip, string>();
        
        private void Awake()
        {
            instance ??= this;
        }
        

        public void DictionarySorting(List<AudioClip> audioList)
        {
            Debug.Log("Dictionary sorting...");
            for (int i = 0; i < audioList.Count; i++)
            {
                SfxDataBase.Add(audioList[i], audioList[i].name);
                Debug.Log(SfxDataBase.Count);
            }
        }
    }
}
