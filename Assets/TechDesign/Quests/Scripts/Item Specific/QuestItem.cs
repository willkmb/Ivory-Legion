using UnityEngine;

namespace Quests
{
    public class QuestItem : MonoBehaviour
    {
        [SerializeField] private int itemID;
        public int itemAmount;

        private void Start()
        {
            AddItemIDToDataBase();
        }

        private void AddItemIDToDataBase()
        {
            QuestManager.instance.AdjustItemToQuestInventory(itemID, itemAmount);
        }

        public int GetItemID()
        {
            return itemID;
        }

        public int GetAddItemAmount()
        {
            return itemAmount;
        }
        
    }
}

