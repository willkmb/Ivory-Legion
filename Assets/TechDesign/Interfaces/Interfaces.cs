using UnityEngine;

namespace Interfaces
{
    public class Interfaces : MonoBehaviour
    {
        ///// Player /////
        public interface IPlayer
        {
            
        }
        ///// AI /////
        public interface INpc
        {
            
        }
        public interface INpcElephant
        {
        }
        public interface INpcHuman
        {
        }
        ///// Interactions /////
        public interface IInteractable
        { 
            void Interact();
        }
        ///// Quests /////
        public interface IHaveQuest
        { 
            void AddQuest(string questName, bool questCompleted);
        }

        public interface ICheckQuestCompletion
        {
            void EventIfQuestCompleted();
            void OnTriggerEnter(Collider other);
        }
    }
}
