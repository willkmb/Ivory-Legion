using System;
using System.Collections.Generic;
using InputManager;
using Player;
using Quests;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Chests
{
    public enum WhatHappensToItems
    {
        GiveToPlayer,
        PlaceInChest,
    }
    public class Chest : MonoBehaviour
    {
        public WhatHappensToItems whatHappensToItem;
        [Header("Item Lists - item element[position] must match in both Lists.")]
        [SerializeField] private List<int> itemsInChest = new List<int>();
        [SerializeField] private List<int> idAmountsInChest = new List<int>();    
        [Header("Variables")]
        public bool giveItemPhysically;
        
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
            switch (whatHappensToItem)
            {
                case WhatHappensToItems.GiveToPlayer:
                    GiveToPlayer();
                    break;
                case WhatHappensToItems.PlaceInChest:
                    PlaceInChest();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        // Gonna add more versatility to item selected later down the line.
        private void GiveToPlayer()
        {
            if (giveItemPhysically)
            {
                if (ItemStorage.instance.itemsInStorage[0] != null && ItemStorage.instance.itemsInStorage[1] != null && ItemStorage.instance.itemsInStorage[2] != null) 
                { 
                    Debug.Log("No Inv space for item");
                    return;
                }

                if (ItemStorage.instance.itemsInStorage[0] == null)
                {
                    GameObject item = QuestManager.instance.ItemIDDataBase[itemsInChest[0]];
                    ItemStorage.instance.PickUp(item, itemsInChest[0], 1);
                    return;
                }
                if (ItemStorage.instance.itemsInStorage[1] == null)
                {
                    GameObject item = QuestManager.instance.ItemIDDataBase[itemsInChest[1]];
                    ItemStorage.instance.PickUp(item, itemsInChest[0], 1);
                    return;
                }
                if (ItemStorage.instance.itemsInStorage[2] == null)
                {
                    GameObject item = QuestManager.instance.ItemIDDataBase[itemsInChest[2]];
                    ItemStorage.instance.PickUp(item, itemsInChest[0], 1);
                }
            }
            else
            {
                QuestManager.instance.AdjustItemToQuestInventory(itemsInChest[0], idAmountsInChest[0]);
            }
        }

        private void PlaceInChest()
        {
            Debug.Log("Im doing this later - brandon :>");
            QuestItem questItem = ItemStorage.instance.itemsInStorage[0].gameObject.GetComponent<QuestItem>();
            if (questItem != null)
            {
                
            }
        }
    }
}

