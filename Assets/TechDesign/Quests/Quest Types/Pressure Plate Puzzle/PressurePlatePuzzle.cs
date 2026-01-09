using System.Collections.Generic;
using Player;
using Quests;
using UnityEngine;

namespace NS_PressurePlate
{
    public class PressurePlatePuzzle : MonoBehaviour
    {
        public int amountOfPressurePlates;
        [SerializeField] private List<PressurePlate> activePressurePlates;
        
        public void ActivatePressurePlate(PressurePlate pressurePlate, int itemID)
        {
            // Check if they have the correct obj
            GameObject go = ItemStorage.instance.itemsInStorage[0]; // Gets obj in trunk
            if (go == null)
            {
                Debug.Log("No item found in trunk");
                return;
            }
            QuestItem questItem = go.GetComponent<QuestItem>();
            if (questItem != null)
            {
                if (itemID == questItem.GetItemID()) // Trunk OBJ ID : Pressure Plate ID
                {
                    activePressurePlates.Add(pressurePlate); // Adds to completed Pressure plates List
                    pressurePlate.isActive = true;
                    PlaceItemDown();
                    CheckIfPuzzleComplete();
                    return;
                }
                Debug.Log("Incorrect item");
            }
        }

        private void CheckIfPuzzleComplete()
        {
            if (amountOfPressurePlates >= activePressurePlates.Count)
            {
                Debug.Log("Puzzle complete");
            }
        }

        private void PlaceItemDown()
        {
            
        }
    }
 
}
