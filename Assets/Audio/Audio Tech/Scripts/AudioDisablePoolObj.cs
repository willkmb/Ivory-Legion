using UnityEngine;

namespace Audio
{
    public class AudioDisablePoolObj : MonoBehaviour
    {
        public AudioSource audioSource;

        public void DisableObj()
        {
            audioSource.Stop();
            AudioManager.instance.audioPoolFree.Add(gameObject);
            gameObject.SetActive(false);
        }
    }
}
