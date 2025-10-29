using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Interfaces.Interfaces;

public class PlayerInteractScript : MonoBehaviour
{
    [Header("Input Bindings")]  //Input Bindings
    [SerializeField] InputAction interact;

    private void Awake()
    {
        //set up input action functionality
        interact.performed += CheckObjectInFront;
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
    private void Update()
    {
        //Debug.DrawRay(new Vector3((transform.position + (transform.forward/4)).x, transform.position.y - 1, (transform.position + (transform.forward / 4)).z), transform.forward, Color.red);
    }
    // Checks if there is an object directly in front of player using raycast
    void CheckObjectInFront(InputAction.CallbackContext context)
    {
       
        Debug.Log("interact");
        RaycastHit hit;
        Vector3 startRay = new Vector3((transform.position + (transform.forward / 4)).x, transform.position.y - 1, (transform.position + (transform.forward / 4)).z);
        if (Physics.Raycast(startRay, transform.forward, out hit, 2f))
        {
            Debug.Log(hit.collider.gameObject.name);
            InteractWithObject(hit.collider.gameObject); // if there is, see if it interactable
        }
    }

    // checks if the object is interactable
    void InteractWithObject(GameObject objectInteracted)
    {
        if (objectInteracted.TryGetComponent(out IInteractable interactableObject))
        {
            interactableObject.Interact(); // runs interaction functionality
        }
    }
}
