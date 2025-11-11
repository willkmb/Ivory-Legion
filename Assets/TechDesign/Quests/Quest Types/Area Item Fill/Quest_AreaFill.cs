using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Quests
{
    public class Quest_AreaFill : MonoBehaviour
    {
        [Header("Items")]
        [SerializeField] private List<int> itemsRequired = new List<int>();
        public List<int> itemsRequiredUsedList = new List<int>();
        public List<GameObject> currentItemsInArea = new List<GameObject>();
        
        [Header("Quest Completion Events")]
        [SerializeField] private QuestCompletionEvent questCompletionEvent;
        [SerializeField] private List<GameObject> unlockAreas = new List<GameObject>();

        private void Awake()
        {
          //  itemsRequiredUsedList = itemsRequired;
        }

        public void CheckItemRequired(PickUpPutDownScript script,GameObject itemObj, int itemID)
        {
           script.questAreaList.Add(this);

           if (itemsRequired.Contains(itemID))
           {
               if (!currentItemsInArea.Contains(itemObj))
               {
                   currentItemsInArea.Add(itemObj);
                   itemsRequiredUsedList.Remove(itemID);
               }
           }
           
           if (itemsRequiredUsedList.Count <= 0)
               ItemsCollected();
        }

        private void ItemsCollected()
        {
            Debug.Log("Items collected");
            switch (questCompletionEvent)
            {
                case QuestCompletionEvent.UnlockArea:
                    foreach (GameObject unlockArea in unlockAreas)
                        unlockArea.SetActive(false);
                    break;
                case QuestCompletionEvent.NpcStopsGuarding:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddQuest(string questName, bool questCompleted)
        {
            QuestManager.instance.questDataBase.Add(questName, questCompleted);
        }
    }
}

