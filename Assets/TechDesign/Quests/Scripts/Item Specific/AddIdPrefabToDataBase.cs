using Quests;
using UnityEngine;

namespace items
{
    public class AddIdPrefabToDataBase : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField] private int id = 0;
        [SerializeField] private int itemAmount = 1;

        [Tooltip("Only applies to pocket Inv")] [SerializeField] private bool hasOnSpawn = false;

        private void Start()
        {
            QuestManager.instance.SetItemDataBase(id, gameObject);
            if (hasOnSpawn)
                QuestManager.instance.AdjustItemToQuestInventory(id, itemAmount);
        }
    }
}

