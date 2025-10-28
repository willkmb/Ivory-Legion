using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SeismicSenseScript : MonoBehaviour
{
    //public GameObject Player;
    public GameObject seismicPulseSphere; 
    public GameObject returnPulse;
    public GameObject particlesObj;
    GameObject Detectable;

    public float pulseSpeed = 0.025f;

    Vector3 SphereExpand;
    Vector3 OriginalScale;
    
    public ParticleSystem particEffects;
    public Material mainMat;
    
    bool activePulse;

    [Header("Input Bindings")]  //Input Bindings
    //  input for getting released button
    [SerializeField] InputAction seismicSense;

    private void Awake()
    {
        //set up input action functionality
        seismicSense.performed += StartPulse;
        seismicSense.canceled += TimerReset;
    }
    private void OnEnable()
    {
        // enables inputs
        seismicSense.Enable();
    }
    private void OnDisable()
    {
        // disables inputs
        seismicSense.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particEffects.Stop();
        SphereExpand = new Vector3(pulseSpeed, pulseSpeed, pulseSpeed);
        OriginalScale = seismicPulseSphere.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        var particSettings = particEffects.main;
        if (Input.GetKey(KeyCode.Space)) //When the 'Space' key is held down, the  
        {

            //particMain.loop = true;
            //partic.Play();
            //activePulse = true;
            if (!activePulse) // if the pulse isn't active, then the pulse will now start and the particle effect will loop
            {
                particSettings.loop = true;
                particEffects.Play();   
                activePulse = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) // Once the 'Space' key is lifted, the timer will begin to reset the pulse
        {
            if (activePulse) // if the pulse is active, then the particle effect will stop looping and start the reset timer
            {
                particSettings.loop = false;
                StartCoroutine(Timer());
            }
        }

        if (activePulse)
        {
            SeismicSen();
        }
        */
    }

    void StartPulse(InputAction.CallbackContext context)
    {
        var particSettings = particEffects.main;
        if (!activePulse) // if the pulse isn't active, then the pulse will now start and the particle effect will loop
        {
            particSettings.loop = true;
            particEffects.Play();
            activePulse = true;
        }
        else
        {
            SeismicSen();
        }
    }

    void TimerReset(InputAction.CallbackContext context)
    {
        var particSettings = particEffects.main;
        if (activePulse) // if the pulse is active, then the particle effect will stop looping and start the reset timer
        {
            particSettings.loop = false;
            StartCoroutine(Timer());
        }
    }


    // Checks whether a detectable object has entered the sphere trigger
    private void OnTriggerEnter(Collider detectObj)
    {
        Debug.Log(detectObj.gameObject);
        Detectable = detectObj.gameObject;

        Debug.Log(Detectable.gameObject);

        if (Detectable.CompareTag("NPC"))
        {
            Instantiate(returnPulse, Detectable.gameObject.transform.localPosition, Quaternion.identity); // Spawns a pulse emited from the location of the detectable object
        }
    }

    // returns the sphere and particles to their original sizes once the timer has ended
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1.5f);
        seismicPulseSphere.transform.localScale = OriginalScale;
        particlesObj.transform.localScale = OriginalScale;
        activePulse = false;
    }

    // Causes the sphere and particle effect to expand outward
    void SeismicSen()
    {
        float rangeMax = 100f;

        seismicPulseSphere.transform.localScale += SphereExpand;
        particlesObj.transform.localScale = seismicPulseSphere.transform.localScale;

        if (seismicPulseSphere.transform.localScale.x > rangeMax) // if the scale of the sphere trigger is greater than the max range, then the sphere is returned to its original scale
        {
            seismicPulseSphere.transform.localScale = OriginalScale;
        }
    }
}
