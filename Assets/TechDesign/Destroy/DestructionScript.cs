using Audio;
using InputManager;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Interfaces.Interfaces;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Splines.Interpolators;
using System.Collections;
using Mechanic_Destruction;


public class DestructionScript : MonoBehaviour, IInteractable
{
    public static DestructionScript instance;

    [Header("Destruction Sound Names - HAVE TO BE EXACT TO AUDIO CLIP NAME")]
    [SerializeField] private string StartSoundFileName;
    [SerializeField] private string ReadySoundFileName;
    [SerializeField] private string DestroySoundFileName;
    [SerializeField] private string FailSoundFileName;

    [HideInInspector] public float timer;
    bool isDestroyed;
    bool isStartDestruct;
    bool readySoundPlayed;
    GameObject player;

    [SerializeField] Material OffMaterial;
    [SerializeField] Material OnMaterial;
    [SerializeField] Material DefaultMaterial;

    [Header("Input Bindings")]  //Input Bindings
    //  input for getting released button
    [SerializeField] InputAction interact;

    private MeshRenderer _meshRenderer;

    [Header("UI")]
    [SerializeField] private Image chargeFill;
    [SerializeField] private float chargeTime; // max time to reach fully charged
    [SerializeField] private Color col1;
    [SerializeField] private Color col2;
    bool revertCharge = false;
    bool revertChargeEnd = false;

    //Spawn obj on destroy
    private ObjSpawnOnBreak _objSpawnOnBreak;
    private void Awake()
    {
        instance ??= this;
        
        isDestroyed = false;
        _meshRenderer = GetComponent<MeshRenderer>();
        _objSpawnOnBreak = transform.GetComponent<ObjSpawnOnBreak>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartDestruct) // if the player has interacted
        {
            timer += Time.deltaTime; //start timer
            if(!revertChargeEnd) chargeFill.fillAmount = Mathf.Lerp(chargeFill.fillAmount, timer / chargeTime, Time.deltaTime * 4f);
            chargeFill.color = Color.Lerp(col1, col2, chargeFill.fillAmount);
           
            if (timer > 2 && timer < 3)
            {
                if (!readySoundPlayed)
                {
                    //PlaySoundReady();
                }
                _meshRenderer.material = OnMaterial; // changes material to indicate correct time to release button
            }
            else
            {
                _meshRenderer.material = OffMaterial; // changes material to indicate incorrect time to release button
            }
        }

        revert();
        revertEnd();
        if (chargeFill.fillAmount <= 0) revertCharge = false;
    }

    // on interact, sets interaction check to true
    public void Interact()
    {
        if (!isDestroyed)
        {
            Debug.Log(isStartDestruct + " - start destruct");
            //PlaySoundStart();
            isStartDestruct = true;
            PlayerManager.instance.currentDestructableObject = this;
            revertCharge = false;
        }
    }

    // on interact release, destroy object if timed right
    public void DestroyObj()
    {
        if (timer > 2 && timer < 3)
        {
            _objSpawnOnBreak.SpawnObj();
            revertChargeEnd = true;
            isDestroyed = true;
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            FindFirstObjectByType<CameraShake>().Shake();
            Invoke("remove", 1.5f);

            //alternative functionality - removess collider and changes material instead of disabling object
            //gameObject.GetComponent<Collider>().enabled = false;
            //gameObject.GetComponent<MeshRenderer>().material = OffMaterial;
            //PlaySoundDestroy();

        }
        else
        {
            //PlaySoundFail();
            timer = 0;
            isStartDestruct = false;
            readySoundPlayed = false;
            _meshRenderer.material = DefaultMaterial;
            revertCharge = true;
        }
    }

    void revert()
    {
        if (revertCharge && !Input.GetKeyDown(KeyCode.JoystickButton3)) { chargeFill.fillAmount = Mathf.Lerp(chargeFill.fillAmount, 0f, Time.deltaTime * 3f); }
    }

    void revertEnd()
    {
        if (revertChargeEnd) { chargeFill.fillAmount = Mathf.Lerp(chargeFill.fillAmount, 0f, Time.deltaTime * 6f); }
    }

    void remove()
    {
        this.gameObject.SetActive(false);
    }

    void PlaySoundStart()
    {
        Debug.Log("playsound");
        AudioManager.instance.PlayAudio(StartSoundFileName, transform.position, false, false, false, 1.0f, 1.0f, true, 0.75f, 1.25f, 128);
    }
    void PlaySoundReady()
    {
        Debug.Log("playsound");
        AudioManager.instance.PlayAudio(ReadySoundFileName, transform.position, false, false, false, 1.0f, 1.0f, true, 0.75f, 1.25f, 128);
        readySoundPlayed = true;
    }
    void PlaySoundDestroy()
    {
        Debug.Log("playsound");
        AudioManager.instance.PlayAudio(DestroySoundFileName, transform.position, false, false, false, 1.0f, 1.0f, true, 0.75f, 1.25f, 128);
    }
    void PlaySoundFail()
    {
        Debug.Log("playsound");
        AudioManager.instance.PlayAudio(FailSoundFileName, transform.position, false, false, false, 1.0f, 1.0f, true, 0.75f, 1.25f, 128);
    }
}
