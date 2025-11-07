using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class ReturnSeismicPulse : MonoBehaviour
{
    public GameObject SeismicPulseSphere;
    public GameObject particlesObj;

    GameObject PlayerChar;

    ParticleSystem particEffects;
    Vector3 SphereExpand = new Vector3(0.05f, 0.05f, 0.05f);
    [Range(0.1f, 0.5f)] public float pulseSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particEffects = particlesObj.GetComponent<ParticleSystem>();
        StartCoroutine(SeismicTimer());
    }

    // Update is called once per frame
    void Update()
    {
        SeismicSense();
        if (particEffects.isStopped) // Destroys the entire object when the particle system is stopped once it's duration ends after looping set to false
        {
            //  Destroy(SeismicPulseSphere);
            SeismicManager.instance.freeReturnPulses.Add(gameObject);
            SeismicManager.instance.inactiveReturnPulses.Remove(gameObject);
            gameObject.transform.parent = SeismicManager.instance.transform;
            gameObject.SetActive(false);
        }
    }

    // Causes the sphere trigger and particle effect to expand outward
    void SeismicSense()
    {
        //SeismicPulseSphere.transform.localScale += SphereExpand;
        SeismicPulseSphere.transform.localScale += new Vector3(pulseSpeed, pulseSpeed, pulseSpeed);
        particlesObj.transform.localScale = SeismicPulseSphere.transform.localScale;
    }

    // This timer is so that the particle effect ends if it has taken too long to reach the player
    private IEnumerator SeismicTimer()
    {
        yield return new WaitForSeconds(5);
        var particSettings = particEffects.main;
        particSettings.loop = false; // Ends the loop, allowing the particle effects duration to cease
    }

    // Checks wether the player has entered the trigger sphere, if it has, then particle looping ends
    private void OnTriggerEnter(Collider player)
    {
        //        Debug.Log(player.gameObject);
        PlayerChar = player.gameObject;

        //        Debug.Log(PlayerChar.gameObject);

        if (PlayerChar.gameObject.CompareTag("Player"))
        {
            var particMain = particEffects.main;
            particMain.loop = false;

        }
    }
}
