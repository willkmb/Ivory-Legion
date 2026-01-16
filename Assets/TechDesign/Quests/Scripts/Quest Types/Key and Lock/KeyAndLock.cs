using System;
using System.Collections.Generic;
using InputManager;
using Player;
using Quests;
using UnityEngine;
using UnityEngine.InputSystem;


namespace NS_KeyAndLock
{
    public enum KeyUsedResult
    {
        TransferScene,
        OpenDoor,
        CloseDoor
    }
    public class KeyAndLock : MonoBehaviour
    {
        [Header("variables")]
        [SerializeField] private int itemIdRequired;
        [SerializeField] private int itemAmountRequired;
        [SerializeField] private GameObject itemPlacedPos;
        
        [Header("")]
        public KeyUsedResult keyUsedResult;
        [SerializeField] private int sceneNumber;
        [SerializeField] private List<GameObject> doors;
        [SerializeField] private GameObject textTrigger;

        [Header("Audio Event Names")] 
        [SerializeField] private string incorrectEventName;
        [SerializeField] private string pickUpObjEventName;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.transform.GetComponent<PlayerManager>() != null)
            {
                ItemStorage.instance.blockPutDown = true;
                PlayerManager.instance.interactAction.performed += Interact;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.transform.GetComponent<PlayerManager>() != null)
            {
                ItemStorage.instance.blockPutDown = false;
                PlayerManager.instance.interactAction.performed -= Interact;
            } 
        }
        private void Interact(InputAction.CallbackContext context)
        {
            GameObject go = ItemStorage.instance.itemsInStorage[0]; // Gets obj in trunk
            if (go == null)
            {
                Debug.Log("No item found in trunk position [0]");
                return;
            }
            QuestItem questItem = go.GetComponent<QuestItem>();
            if (questItem != null)
            {
                if (itemIdRequired == questItem.GetItemID() /*&& QuestManager.instance.PlayerInventory[questItem.GetItemID()] >= itemAmountRequired*/) // Will test this commented part out later
                {
                    PlaceItemDown(go);
                    CorrectKeyPlaced();
                    return;
                }
                Debug.Log("Incorrect item");
            }
        }
        private void PlaceItemDown(GameObject questObj)
        {
            ItemStorage.instance.ForcePutDownTrunkObj(itemPlacedPos.transform.position);
            PickUpPutDownScript pickUpScript = questObj.GetComponent<PickUpPutDownScript>();
            if (pickUpScript != null)
                pickUpScript.enabled = false;
        }

        private void CorrectKeyPlaced()
        {
            if (textTrigger != null)
               textTrigger.transform.gameObject.SetActive(false);
            switch (keyUsedResult)
            {
                case KeyUsedResult.TransferScene:
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
                    break;
                case KeyUsedResult.OpenDoor:
                    foreach (GameObject door in doors)
                        door.SetActive(false);
                    break;
                case KeyUsedResult.CloseDoor:
                    foreach (GameObject door in doors)
                        door.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    } 
}

