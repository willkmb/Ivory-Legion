using InputManager;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace  NS_PressurePlate
{
    public class PressurePlate : MonoBehaviour
    {
        [Header("Main Pressure Plate Puzzle Obj")]
        [SerializeField] private PressurePlatePuzzle pressurePlatePuzzle;
        [Header("Variables")]
        public bool isActive;
        [SerializeField] private int requiredItemID;
        private void OnTriggerEnter(Collider other)
        {
            if (isActive)
                return;
            if(other.gameObject.transform.GetComponent<PlayerManager>() != null)
            {
                // Sees if the required OBj is the right spot, if it is, the pressure plate is ACTIVE (completed)
                // Also checks if all the pressure plates have been completed for that puzzle
                ItemStorage.instance.blockPutDown = true;
                PlayerManager.instance.interactAction.performed += Interact;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isActive)
                return;
            if(other.gameObject.transform.GetComponentInParent<PlayerManager>() != null)
            {
                // Sees if the required OBj is the right spot, if it is, the pressure plate is ACTIVE (completed)
                // Also checks if all the pressure plates have been completed for that puzzle
                ItemStorage.instance.blockPutDown = false;
                PlayerManager.instance.interactAction.performed -= Interact;
            }
        }

        private void Interact(InputAction.CallbackContext context)
        {
            Debug.Log("Interact");
            pressurePlatePuzzle.ActivatePressurePlate(this, requiredItemID);
        }
    }
}

