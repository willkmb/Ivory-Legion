using System;
using InputManager;
using UnityEngine;
using UnityEngine.InputSystem;
using static Interfaces.Interfaces;


public class DestructionScript : MonoBehaviour, IInteractable
{
    public static DestructionScript instance;
    
    [HideInInspector] public float timer;
    bool isDestroyed;
    bool isStartDestruct;
    GameObject player;

    [SerializeField] Material OffMaterial;
    [SerializeField] Material OnMaterial;
    [SerializeField] Material DefaultMaterial;

    [Header("Input Bindings")]  //Input Bindings
    //  input for getting released button
    [SerializeField] InputAction interact;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        instance ??= this;
        
        isDestroyed = false;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartDestruct) // if the player has interacted
        {
            timer += Time.deltaTime; //start timer
           
            if (timer > 2 && timer < 3)
            {
                _meshRenderer.material = OnMaterial; // changes material to indicate correct time to release button
            }
            else
            {
                _meshRenderer.material = OffMaterial; // changes material to indicate incorrect time to release button
            }
        }
    }

    // on interact, sets interaction check to true
    public void Interact()
    {
   
        if (!isDestroyed)
        {
            Debug.Log(isStartDestruct + " - start destruct");
            isStartDestruct = true;
            PlayerManager.instance.currentDestructableObject = this;
        }
    }

    // on interact release, destroy object if timed right
    public void DestroyObj()
    {
        if (timer > 2 && timer < 3)
        {
            isDestroyed = true;

                //alternative functionality - removess collider and changes material instead of disabling object
            //gameObject.GetComponent<Collider>().enabled = false;
            //gameObject.GetComponent<MeshRenderer>().material = OffMaterial;

            gameObject.SetActive(false);
        }
        else
        {
             timer = 0;
             isStartDestruct = false;
             _meshRenderer.material = DefaultMaterial;
        }
    }
}
