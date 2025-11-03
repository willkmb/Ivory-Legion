using System;
using Npc.AI;
using UnityEngine;

namespace Quests
{
    public class Quest_DialogueAlterer : MonoBehaviour
    {
        public QuestType questType;

        // Scripts
        private NpcManager _npcManager;
        
        private Quest_ItemRetrieval _questItemRetrieval;
        private Quest_ItemChecker _questItemChecker;
        
        private Quest_PlayerAlteration _questPlayerAlteration;
        
        private void Awake()
        {
           _npcManager = transform.GetComponent<NpcManager>();
           _npcManager.questDialogueAlterer = this;
            switch (questType)
            {
                case QuestType.ItemReterival:
                    _questItemRetrieval = transform.GetComponent<Quest_ItemRetrieval>();
                    _questItemChecker = transform.GetComponent<Quest_ItemChecker>();
                    break;
                case QuestType.PlayerVisualAlteration:
                    _questPlayerAlteration = transform.GetComponent<Quest_PlayerAlteration>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        // Only called in the NPC MANAGER
        // Checks if the quest has been completed - Checks based of the type of quest that has been applied to the game object
        public void ChangeDialogueBasedOnQuests()
        {
            switch (questType)
            {
                case QuestType.ItemReterival:
                    // Items not collected 
                    //If there is an item quest for this NPC, and checks if it has been completed
                    if(_questItemChecker != null && !QuestManager.instance.CheckIfQuestCompleted(_questItemChecker.questItemRetrieval.questName))
                    {
                        _questItemChecker.CheckIfItemsCollected(); // Checks if the correct items have been completed
                        if (QuestManager.instance.CheckIfQuestCompleted(_questItemChecker.questItemRetrieval.questName))
                        {
                            // CHANGE DIALOGUE TO QUEST COMPLETION ONES
                            _questItemChecker.EventIfQuestCompleted();   // Change to when dialogue clicked do this script
                            return;
                        }
                        //Dialogue of NPC telling them to do the quest
                        Debug.Log("Quest not completed go get" + _questItemRetrieval.itemsToRetrieve.ToString() + "!");
                        return;
                    }
                    //Item(s) collected - Sets quest is completed
                    if(_questItemChecker != null && QuestManager.instance.CheckIfQuestCompleted(_questItemChecker.questItemRetrieval.questName))
                    {
                        // New dialogue for after the quest is completed
                        Debug.Log("Quest completed now leave me alone!!");
                    }
                    break;
                case QuestType.PlayerVisualAlteration:
                    if (_questPlayerAlteration != null)
                    {
                        _questPlayerAlteration.CheckPlayerAlteration();
                        if (QuestManager.instance.CheckIfQuestCompleted(_questPlayerAlteration.questName))
                        {
                            // CHANGE DIALOGUE TO QUEST COMPLETION ONES
                            _questPlayerAlteration.EventIfQuestCompleted();
                            return;
                        }
                        //Dialogue of NPC telling them to do the quest
                        Debug.Log("You ain't wearing the correct outfit!!");
                        return;
                    }
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

