using UnityEngine;

namespace Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        public AudioSource audioSource;
        public void DisableObj()
        {
            audioSource.Stop();
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
            AudioManager.instance.audioPoolFree.Add(gameObject);
            gameObject.SetActive(false);
        }
    }
}
