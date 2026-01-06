using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioFoliageReactor : MonoBehaviour
    {
        [SerializeField] private List<string> audioEventList;
        public string FetchList()
        {
            string randomString = audioEventList[Random.Range(0, audioEventList.Count)];
            return randomString;
        }
    }
}

