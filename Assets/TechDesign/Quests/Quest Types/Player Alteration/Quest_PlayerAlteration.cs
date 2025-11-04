using System;
using InputManager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quests
{
    public class Quest_PlayerAlteration : MonoBehaviour, Interfaces.Interfaces.IHaveQuest, Interfaces.Interfaces.ICheckQuestCompletion
    {
        [Header("Be specific with quest names")]
        public string questName;
        [Header("Mesh Filters")]
        public Mesh playerMeshRenderer; // Add the players meshes to some form of dictionary 
        public Mesh questMeshRenderer;  // Can be altered with "ChangeQuestMesh" function

        [Header("Quest Completion Objs")] 
        [SerializeField] private GameObject blocker;
        
        public QuestCompletionCheckers questCompletionCheckers;
        public QuestCompletionEvent questCompletionEvent;
        
        private void Start()
        {
            AddMeshToDataBase(questMeshRenderer.name, questMeshRenderer);
        }
        public void AddMeshToDataBase(string meshName, Mesh mesh) // Public as it might be used later down the line
        {
            if(!QuestManager.instance.playerMeshesDataBase.TryGetValue(questName, out var value)) // Stops duplication of quests between scenes
                  QuestManager.instance.SetPlayerMeshDataBase(meshName, mesh);
        }

        public void ChangeQuestMesh(string meshName, Mesh mesh)
        {
            //Checks if mesh is in the playerMeshesDataBase, if not add it
            if (!QuestManager.instance.playerMeshesDataBase.TryGetValue(meshName, out var value))
                AddMeshToDataBase(meshName, mesh);
            
            // Sets the questMeshRenderer from the string (meshName)
            QuestManager.instance.playerMeshesDataBase.TryGetValue(meshName, out var newMeshRenderer);
            questMeshRenderer = newMeshRenderer;
        }
        public void AddQuest(string questName, bool questCompleted) // Public as it might be used later down the line
        {
            if (!QuestManager.instance.questDataBase.TryGetValue(questName, out var value)) // Stops duplication of quests between scenes
                QuestManager.instance.SetQuestDataBase(questName, questCompleted); // Adds quest by name and sets it to false;
        }

        public void CheckPlayerAlteration()
        {
            // Check if the Player's visuals matches the required visuals for quest
            if (playerMeshRenderer == questMeshRenderer)
                if (!QuestManager.instance.CheckIfQuestCompleted(questName))
                {
                    QuestManager.instance.QuestCompletedSetToTrue(questName);
                    EventIfQuestCompleted();
                }
        }
        
        // Checks if player is visually changed based of Trigger
        public void OnTriggerEnter(Collider other)
        {
            if (questCompletionCheckers == QuestCompletionCheckers.Trigger)
                if (other.CompareTag("Player") && QuestManager.instance.CheckIfQuestCompleted(questName))
                    CheckPlayerAlteration();
        }

        public void EventIfQuestCompleted()
        {
            switch (questCompletionEvent)
            {
                case QuestCompletionEvent.UnlockArea:
                    Debug.Log("Unlock Area");
                    blocker.SetActive(false);
                    break;
                case QuestCompletionEvent.NpcStopsGuarding:
                    Debug.Log("Npc Stops Guarding");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

