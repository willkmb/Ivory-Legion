using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioFoliageReactor : MonoBehaviour
    {
        [SerializeField] private List<string> audioEventList;
        [SerializeField] private bool reverbCheck;
        [SerializeField] private bool is3d;
        public string FetchList()
        {
            string randomString = String.Empty;
            
            if(audioEventList.Count <= 1)
                randomString = audioEventList[0];
            else
                randomString = audioEventList[Random.Range(0, audioEventList.Count - 1)];
            
            return randomString;
        }

        public bool ReverbCheck()
        {
            return reverbCheck;
        }

        public bool Is3D()
        {
            return is3d;
        }
    }
}

