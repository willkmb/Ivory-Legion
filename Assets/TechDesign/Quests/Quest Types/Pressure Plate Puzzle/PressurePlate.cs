using System;
using InputManager;
using UnityEngine;

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
                pressurePlatePuzzle.ActivatePressurePlate(this, requiredItemID);
            }
        }
    }
}

