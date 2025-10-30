using System.Collections;
using InputManager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SeismicSense
{
    public class SeismicSenseScript : MonoBehaviour
    {
        public static SeismicSenseScript instance; //Singleton (can be called from other scripts to reference this one
        
        [Header("Pulse Items")]
        [SerializeField] private GameObject seismicPulseSphere;
        [SerializeField] private GameObject returnPulse;
        [SerializeField] private GameObject particlesObj;
        public ParticleSystem particleEffects;
        private GameObject _detectable;

        [Header("Values")] 
        [Range(0.1f, 0.5f)] public float pulseSpeed;
        [Range(5f, 100f)] public float rangeMax = 100f;
        
        // Variables
        [HideInInspector] public bool inProgress;

        // Trigger Sphere Values
        Vector3 _sphereExpand;  // How much it expands by every frame 
        Vector3 _originalScale;

        private void Awake()
        {
            instance ??= this; // Setting singleton referencing 
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            particleEffects.Stop();
            _sphereExpand = new Vector3(pulseSpeed, pulseSpeed, pulseSpeed); // Sets how much it expands by every frame
            _originalScale = seismicPulseSphere.transform.localScale; // For resetting purposes
        }

        // Update is called once per frame
        void Update()
        {
            if (inProgress) //Start sphere expansion for detection of objs
                SeismicSen();
        }

        public void StartPulse()
        {
            var particle = particleEffects.main;
            particle.loop = true;
            particleEffects.Play();
        }

        public void Reset()
        {
            var particSettings = particleEffects.main;

            inProgress = false;
            particSettings.loop = false;
            
            seismicPulseSphere.transform.localScale = _originalScale;
            particlesObj.transform.localScale = _originalScale;
        }
        
        // Checks whether a detectable object has entered the sphere trigger
        private void OnTriggerEnter(Collider detectObj)
        {
            _detectable = detectObj.gameObject;
            
            if (_detectable.CompareTag("NPC"))
            {
                Instantiate(returnPulse, _detectable.gameObject.transform.localPosition, Quaternion.identity);
                // Spawns a pulse emitted from the location of the detectable object
            }
        }

        // Causes the sphere and particle effect to expand outward
        private void SeismicSen()
        {
            seismicPulseSphere.transform.localScale += _sphereExpand; // Trigger sphere expansion 
            particlesObj.transform.localScale = seismicPulseSphere.transform.localScale; // Ensure particles match up with detection

            // if the scale of the sphere trigger is greater than the max range, then the sphere is returned to its original scale and cooldown is reset
            if (seismicPulseSphere.transform.localScale.x > rangeMax)
            {
                inProgress = false;
                Reset();
                PlayerManager.instance.seismicOffCooldown = true;
            }
        }
        
        // returns the sphere and particles to their original sizes once the timer has ended
        // private void Reset()
        // {
        //    // yield return new WaitForSeconds(1.5f);
        //     seismicPulseSphere.transform.localScale = OriginalScale;
        //     particlesObj.transform.localScale = OriginalScale;
        //     activePulse = false;
        //
        //     PlayerManager.instance.seismicOffCooldown = true;
        // }
        
        
        /*var particSettings = particEffects.main;
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
        }*/

        // if (activePulse)
        // {
        //     SeismicSen();
        // }
    }
}

