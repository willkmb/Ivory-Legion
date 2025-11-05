using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public enum QuestType
    {
        ItemReterival,
        PlayerVisualAlteration,
    }
    public enum QuestCompletionCheckers
    {
        Dialogue, // When talking to an AI check if you x items before being able to advance
        Trigger, // When you enter the trigger check you have x items
    }
    public enum QuestCompletionEvent
    {
        UnlockArea,
        NpcStopsGuarding,
    }
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance;
        
        public Dictionary<string, bool> questDataBase = new Dictionary<string, bool>(); // A list of the quests and their completion value
        public Dictionary<int, GameObject> questItemIDDataBase = new Dictionary<int, GameObject>(); // Used to call ID of item, to beu sed in various quests
        public Dictionary<int, int> questPlayerInventory = new Dictionary<int, int>(); // ITEM ID : ITEM COUNT
        
        public Dictionary<string, Mesh> playerMeshesDataBase = new Dictionary<string, Mesh>(); // ITEM ID : ITEM COUNT

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            if (instance == null)
                instance ??= this; // Singleton setter
        }
        public void SetQuestDataBase(string questName, bool value)
        {
            questDataBase.Add(questName, value);
            Debug.Log(questName);
        }

        public void SetItemDataBase(int itemID, GameObject item)
        {
            questItemIDDataBase.Add(itemID, item);
        }

        public void SetPlayerMeshDataBase(string playerID, Mesh mesh)
        {
            playerMeshesDataBase.Add(playerID, mesh);
        }

        public void QuestCompletedSetToTrue(string questName)
        {
            if(questDataBase.TryGetValue(questName, out var questBool))
            {
                questDataBase.Remove(questName, out var clip);
                questDataBase.Add(questName, true);
                Debug.Log("Quest Completed ");
                return;
            }
            Debug.LogError(questName + " doesn't exist");
        }
        public void QuestCompletedSetToFalse(string questName)
        {
            if(questDataBase.TryGetValue(questName, out var questBool))
            {
                questDataBase.Remove(questName, out var clip);
                questDataBase.Add(questName, false);
                Debug.Log("Quest Not Completed");
                return;
            }
            Debug.LogError(questName + " doesn't exist");
        }

        public bool CheckIfQuestCompleted(string questName)
        {
            if(questDataBase.TryGetValue(questName, out var questBool))
            {
                Debug.Log("Quest Completion Status = " + questBool);
                return questBool;
            }
            Debug.LogError(questName + " doesn't exist");
            return false;
        }
    }
}
