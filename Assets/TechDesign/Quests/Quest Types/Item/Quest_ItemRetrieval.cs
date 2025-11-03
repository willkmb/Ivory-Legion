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


       private void Start()
       {
           //Adds quests if it doesn't exist   
           if (!QuestManager.instance.questDataBase.TryGetValue(questName, out var value)) // Stops duplication of quests between scenes
                AddQuest(questName, false);
           
           if (!QuestManager.instance.questDataBase.ContainsKey(questName)) // Stops duplication of quests items s scenes
               SetItemIDs(itemListIDValues, itemsToRetrieve); // Sets values to the items in the List - First item in list = 0, second item = 1, etc

           AddItemToQuestInventory(0, 1); // Testing
           AddItemToQuestInventory(1,1); // Testing
           AddItemToQuestInventory(2,1); // Testing
           AddItemToQuestInventory(2,1); // Testing
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
        public void AddItemToQuestInventory(int itemID,int amountToAdd)
        { 
            //If the ID is already in the Quest Item DataBase +x the amount of that ID collected into the players inventory
            if (QuestManager.instance.questPlayerInventory.ContainsKey(itemID)) 
            {
                // +1 to inventory amount of that quest item
                if (QuestManager.instance.questPlayerInventory.Remove((itemID), out var itemAmount))
                {
                    var newAmount = itemAmount + amountToAdd;
                    QuestManager.instance.questPlayerInventory.Add((itemID), newAmount);
                }
                return;
            }
           
            QuestManager.instance.questPlayerInventory.Add((itemID), 1); // Item ID : Item Amount
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
                QuestManager.instance.QuestCompletedSetToTrue(questName);
        }

        private int CollectItemID(GameObject obj)
        {
            int tempInt = itemsToRetrieve.IndexOf(obj);
            return tempInt = itemListIDValues[tempInt];
        }
    }  
}

