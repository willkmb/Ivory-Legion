using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public enum QuestType
    {
        ItemReterival,
        PlayerVisualAlteration,
        AreaFill,
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
        
        public Dictionary<string, Mesh> playerMeshesDataBase = new Dictionary<string, Mesh>(); // ITEM ID : MESH
        
        [Header("Items in this scene")]
        [Header("Each ID only needed once")]
        [SerializeField] private List<int> itemIds = new List<int>();
        [SerializeField] private List<GameObject> itemsInArea = new List<GameObject>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            if (instance == null)
                instance ??= this; // Singleton setter
        }

        private void Start()
        {
            for (int i = 0; i < itemIds.Count; i++)
            {
               int itemID = itemIds[i];
               GameObject itemObj = itemsInArea[itemID];
               SetItemDataBase(itemID, itemObj);
            }
        }
        public void SetQuestDataBase(string questName, bool value)
        {
            questDataBase.Add(questName, value);
        }

        public void SetItemDataBase(int itemID, GameObject item)
        {
            if (!questDataBase.ContainsKey(itemID.ToString()))
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
                Debug.Log( questName + " Completion Status = " + questBool);
                return questBool;
            }
            Debug.LogError(questName + " doesn't exist");
            return false;
        }

        public void AdjustItemToQuestInventory(int itemID,int amountToAdd)
        { 
            //If the ID is already in the Quest Item DataBase +x the amount of that ID collected into the players inventory
            if (questPlayerInventory.ContainsKey(itemID)) 
            {
                // +1 to inventory amount of that quest item
                if (questPlayerInventory.Remove((itemID), out var itemAmount))
                {
                    var newAmount = itemAmount + amountToAdd;
                    questPlayerInventory.Add((itemID), newAmount);
                }
                return;
            }
           
            questPlayerInventory.Add((itemID), 1); // Item ID : Item Amount
        }
    }
}
