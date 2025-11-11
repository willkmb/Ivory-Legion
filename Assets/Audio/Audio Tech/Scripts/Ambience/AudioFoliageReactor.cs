using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioFoliageReactor : MonoBehaviour
    {
        [SerializeField] private List<string> audioList;
        public List<string> FetchList()
        {
            return audioList;
        }
    }
}

