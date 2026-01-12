using System.Collections.Generic;
using Audio;
using Player;
using Quests;
using UnityEngine;

namespace NS_PressurePlate
{
    public class PressurePlatePuzzle : MonoBehaviour
    {
        public int amountOfPressurePlates;
        [SerializeField] private List<PressurePlate> activePressurePlates;
        [SerializeField] private GameObject rewardObjPrefab;

        [Header("Audio Event Names")]
        [SerializeField] private string incorrectAudioEvent;
        [SerializeField] private string correctAudioEvent;
        [SerializeField] private string puzzleCompletedAudioEvent;
        
        public void ActivatePressurePlate(PressurePlate pressurePlate, int itemID)
        {
            if (activePressurePlates.Contains(pressurePlate))
                return;
            // Check if they have the correct obj
            GameObject go = ItemStorage.instance.itemsInStorage[0]; // Gets obj in trunk
            if (go == null)
            {
                Debug.Log("No item found in trunk position [0]");
                PlayFMODSound(pressurePlate.transform.position,incorrectAudioEvent);
                return;
            }
            QuestItem questItem = go.GetComponent<QuestItem>();
            if (questItem != null)
            {
                if (itemID == questItem.GetItemID()) // Trunk OBJ ID : Pressure Plate ID
                {
                    activePressurePlates.Add(pressurePlate); // Adds to completed Pressure plates List
                    pressurePlate.isActive = true;
                    PlaceItemDown(go, questItem.GetItemID(), pressurePlate);
                    CheckIfPuzzleComplete();
                    return;
                }
                PlayFMODSound(pressurePlate.transform.position,incorrectAudioEvent);
                Debug.Log("Incorrect item");
            }
        }

        private void CheckIfPuzzleComplete()
        {
            if (activePressurePlates.Count >=  amountOfPressurePlates)
            {
                foreach (var obj in activePressurePlates)
                    obj.transform.gameObject.SetActive(false);
                
                rewardObjPrefab.SetActive(true);
                PlayFMODSound(transform.position,puzzleCompletedAudioEvent);
            }
        }

        private void PlaceItemDown(GameObject questObj,int itemID, PressurePlate pressurePlate)
        {
            ItemStorage.instance.ForcePutDownTrunkObj(pressurePlate.transform.position);
            PickUpPutDownScript pickUpScript = questObj.GetComponent<PickUpPutDownScript>();
            if (pickUpScript != null)
                pickUpScript.enabled = false;
            
            PlayFMODSound(pressurePlate.transform.position,correctAudioEvent);
        }

        private void PlayFMODSound(Vector3 pos,string eventName)
        {
            AudioManager.instance.PlayFMODSound(pos, eventName, 1.5f, true, false, false, false, 
                0, 0, false, 0, 0, 
                true, false, null, null);
        }
    }
 
}
