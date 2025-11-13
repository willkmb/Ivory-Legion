using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Quests
{
    public class Quest_ItemRetrieval : MonoBehaviour, Interfaces.Interfaces.IHaveQuest
    {
       [Header("Be specific with quest names")]
       public string questName;
       public List<GameObject> itemsToRetrieve;
       public List<int> itemListIDValues;

       private Quest_ItemChecker _questItemChecker;

       private void Awake()
       {
           _questItemChecker = transform.GetComponent<Quest_ItemChecker>();
       }

       private void Start()
       {
           if (!QuestManager.instance.questDataBase.ContainsKey(questName)) // Stops duplication of quests items s scenes
               SetItemIDs(itemListIDValues, itemsToRetrieve); // Sets values to the items in the List - First item in list = 0, second item = 1, etc
       }
       
       public void AddQuest(string questName, bool questCompleted) 
       { 
           QuestManager.instance.SetQuestDataBase(questName, questCompleted); // Adds quest by name and sets s to false;
       }

        private void SetItemIDs(List<int> itemValues, List<GameObject> itemsToRetrieve)
        {
            for (int i = 0; i < itemsToRetrieve.Count; i++)
            {
                if (!QuestManager.instance.questItemIDDataBase.ContainsKey(itemValues[i])) // Stops duplication preventing two quest items having the same ID
                    QuestManager.instance.SetItemDataBase(itemValues[i], itemsToRetrieve[i]);
            }
        }
        private void AddItemToQuestInventory(int itemID,int amountToAdd)
        { 
            QuestManager.instance.AdjustItemToQuestInventory(itemID, amountToAdd);
        }

        public void CheckIfItemsCollected()
        {
            List<GameObject> tempObjList =  new List<GameObject>();
            List<int> tempIDList =  new List<int>();
            List<int> itemAmountList =  new List<int>();
            int itemsCollected = 0;
            
            foreach (GameObject obj in itemsToRetrieve) // Checks if Player has all items required in their inventory
            {
                int ID = CollectItemID(obj); // Gets the quest items ID
                // If inventory contains the item's ID
                if (QuestManager.instance.questPlayerInventory.TryGetValue(ID, out var itemAmount))
                {
                    tempObjList.Add(obj); // Saves the quest item
                    tempIDList.Add(ID); // Saves ID of quest item
                    itemAmountList.Add(itemAmount); // Saves ID of quest item
                    QuestManager.instance.questPlayerInventory.Remove(ID); // Allows quests require 2+ items, re-added to inventory after the checks
                    QuestManager.instance.questPlayerInventory.Add(ID, itemAmount - 1); // Minus purely for checks
                    itemsCollected += 1;
                }
            }
            // Re adds removed objects from player's quest inventory
            int i = 0;
            foreach (GameObject obj in tempObjList)
            {
                AddItemToQuestInventory(tempIDList[i], itemAmountList[i]);
                i++;
            }

            if (itemsCollected >= itemsToRetrieve.Count) // If all items retrieved set quest to complete
            {
                QuestManager.instance.QuestCompletedSetToTrue(questName);
                    _questItemChecker.EventIfQuestCompleted();
            }
        }

        private int CollectItemID(GameObject obj)
        {
            int tempInt = itemsToRetrieve.IndexOf(obj);
            return tempInt = itemListIDValues[tempInt];
        }
    }  
}

