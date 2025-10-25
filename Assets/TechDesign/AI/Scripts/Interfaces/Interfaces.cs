using System.Collections.Generic;
using Audio;
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
        public interface IHaveDiaSounds
        {
            public void AddDia(List<AudioClip> audioList);
        }
        public interface IHaveMusicSounds
        {
            public void AddMusic(List<AudioClip> audioList);
        }
        public interface IHaveAmbSounds
        {
            public void AddAmb(List<AudioClip> audioList);
        }
    }
}
