using System;
using System.Collections.Generic;
using Npc.AI;
using UnityEngine;

namespace Quests
{
    public class Quest_ItemChecker : MonoBehaviour, Interfaces.Interfaces.ICheckQuestCompletion
    {
        [Header("Item Quest I am Checking")]
        public Quest_ItemRetrieval questItemRetrieval; // Change if a new item quest appears
        public QuestCompletionEvent questCompletionEvent; // Change if a new item quest appears
        [Header("How the quest checks if items collected")]
        [SerializeField] private QuestCompletionCheckers checkerType;

        [Header("Areas cleared when Quest Complete")]
        [SerializeField] private List<GameObject> unlockAreasList;
        [SerializeField] private List<NpcManager> gaurdingNpcs;
        
        public void OnTriggerEnter(Collider other)
        {
            if (checkerType == QuestCompletionCheckers.Trigger)
                if (other.CompareTag("Player") && QuestManager.instance.CheckIfQuestCompleted(questItemRetrieval.questName))
                    questItemRetrieval.CheckIfItemsCollected();
        }

        public void CheckIfItemsCollected()
        {
            questItemRetrieval.CheckIfItemsCollected();
        }

        public void EventIfQuestCompleted()
        {
            switch (questCompletionEvent)
            {
                case QuestCompletionEvent.UnlockArea:
                    foreach (GameObject unlockArea in unlockAreasList)
                        unlockArea.SetActive(false);
                    break;
                case QuestCompletionEvent.NpcStopsGuarding:
                    Debug.Log("NPC stops gaurding");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

