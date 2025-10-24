using System.Collections.Generic;
using Prefabs.Audio.Scripts;
using UnityEngine;

namespace Interfaces
{
    public class Interfaces : MonoBehaviour
    {
        public interface IPlayer
        {
            
        }
        public interface INpc
        {
            
        }
        public interface INpcElephant
        {
        }
        public interface INpcHuman
        {
        }

        public interface IHaveSfxSounds
        {
            public void AddSfx(List<AudioClip> audioList);
        }
    }
}
