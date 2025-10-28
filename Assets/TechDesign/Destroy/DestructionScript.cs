using UnityEngine;
using UnityEngine.InputSystem;
using static Interfaces.Interfaces;


public class DestructionScript : MonoBehaviour, IInteractable
{
    float timer;
    bool isDestroyed;
    bool isStartDestruct;
    GameObject player;

    [SerializeField] Material OffMaterial;
    [SerializeField] Material OnMaterial;
    [SerializeField] Material DefaultMaterial;

    [Header("Input Bindings")]  //Input Bindings
    //  input for getting released button
    [SerializeField] InputAction interact;

    private void Awake()
    {
        //set up input action functionality
        interact.canceled += Destroy;
    }
    private void OnEnable()
    {
        // enables inputs
        interact.Enable();
    }
    private void OnDisable()
    {
        // disables inputs
        interact.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartDestruct) // if the player has interacted
        {
            timer += Time.deltaTime; //start timer
            Debug.Log(timer);

            if (timer > 2 && timer < 3)
            {
                gameObject.GetComponent<MeshRenderer>().material = OnMaterial; // changes material to indicate correct time to release button
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = OffMaterial; // changes material to indicate incorrect time to release button
            }
        }
    }

    // on interact, sets interaction check to true
    public void Interact()
    {
        if (!isDestroyed)
        {
            isStartDestruct = true;
        }
    }

    // on interact release, destroy object if timed right
    void Destroy(InputAction.CallbackContext context)
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
            gameObject.GetComponent<MeshRenderer>().material = DefaultMaterial;
        }
    }

    public void Activate()
    {
        throw new System.NotImplementedException();
    }
}
