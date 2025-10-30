using System;
using InputManager;
using UnityEngine;
using UnityEngine.InputSystem;
using static Interfaces.Interfaces;

namespace Player
{
    public class PlayerInteractScript : MonoBehaviour
    {
        public static PlayerInteractScript instance;
        

        private void Awake()
        {
            instance ??= this;
        }

        // Checks if there is an object directly in front of player using raycast
        public void CheckObjectInFront()
        {
            RaycastHit hit;
            Vector3 startRay = new Vector3((transform.position + (transform.forward / 3.5f)).x, transform.position.y - 1, (transform.position + (transform.forward / 3.5f)).z);
            if (Physics.Raycast(startRay, transform.forward, out hit, 3f))
            {
                if (hit.collider.gameObject.TryGetComponent(out IInteractable interactableObject)) // checks if the object is interactable
                {
                    interactableObject.Interact(); // runs interaction functionality

                    IAmDestructable destructable = hit.transform.GetComponent<IAmDestructable>();
                    if (destructable != null) // To enable the held button functionality of the destroy mechanic
                        PlayerManager.instance.destroyChargeInProgress = true;
                }
            }
        }
    }
}

