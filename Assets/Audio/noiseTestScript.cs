using Audio;
using UnityEngine;

public class noiseTestScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        Debug.Log("playsound");
        AudioManager.instance.PlayAudio("FillerSound", transform.position, false, false, false, 1.0f, 1.0f, true, 1f, 1f, 1);
    }
}
