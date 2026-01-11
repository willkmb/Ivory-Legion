using System;
using InputManager;
using UnityEngine;
namespace Quests
{
    public class Quest_Actvate : MonoBehaviour
    {

        public bool addQuestOnStart;
        public QuestType questType;
        public QuestCompletionCheckers questCompletionCheckers;
        
        
        [Header("Quest Variables")]
        public string questName;
        
        // Scripts
        private Quest_PlayerAlteration _playerAlteration;
        public Quest_ItemRetrieval questItemRetrieval;
        private Quest_AreaFill _areaFill;
        
        private void Start()
        {
            switch(questType)
            {
                case QuestType.ItemReterival:
                    questItemRetrieval = transform.GetComponent<Quest_ItemRetrieval>();
                    break;
                case QuestType.PlayerVisualAlteration:
                    _playerAlteration =  transform.GetComponent<Quest_PlayerAlteration>();
                    break;
                case QuestType.AreaFill:
                    _areaFill = transform.GetComponent<Quest_AreaFill>().GetComponent<Quest_AreaFill>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (addQuestOnStart)
                AddQuest();
        }

        private void AddQuest()
        {
            if (_playerAlteration != null)
                _playerAlteration.AddQuest(questName, false);
            if (questItemRetrieval != null)
                questItemRetrieval.AddQuest(questName, false);
            if (_areaFill != null)
                _areaFill.AddQuest(questName, false);
        }
        
        private void OnTriggerEnter(Collider other)
         {
            if (addQuestOnStart)
                  return;
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
                switch (questCompletionCheckers)
                {
                    case QuestCompletionCheckers.Dialogue:
                        break;
                    case QuestCompletionCheckers.Trigger:
                        if (_playerAlteration != null)
                            _playerAlteration.AddQuest(questName, false);
                        if (questItemRetrieval != null)
                            questItemRetrieval.AddQuest(questName, false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
         }

        public void AddQuestOfDialogue()
        {
            // Talk to Will
        }
    }
}

