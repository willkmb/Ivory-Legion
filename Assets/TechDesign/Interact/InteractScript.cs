using UnityEngine;
using UnityEngine.InputSystem;

public class InteractScript : MonoBehaviour
{
    InputSystem_Actions inputs;
    InputAction interact;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        interact = inputs.Player.Interact;
        interact.Enable();
    }
    private void OnDisable()
    {
        interact.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
