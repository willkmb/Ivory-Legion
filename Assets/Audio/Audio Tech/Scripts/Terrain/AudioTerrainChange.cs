using InputManager;
using Npc.AI;
using UnityEngine;

namespace Audio
{
    public class AudioTerrainChange : MonoBehaviour
    {
        [SerializeField] private string audioEventNameElephant;
        [SerializeField] private string audioEventNameHuman;
        private void OnTriggerEnter(Collider other)
        {
            PlayerManager  player = other.GetComponent<PlayerManager>();
            if (player != null)
                if (!player.audioEventNames.Contains(audioEventNameElephant))
                {
                    player.audioEventNames.Add(audioEventNameElephant);
                    return;
                }
            NpcManager npcManager = other.GetComponent<NpcManager>();
            if (npcManager != null)
                if (!npcManager.audioEventNames.Contains(audioEventNameHuman))
                    npcManager.audioEventNames.Add(audioEventNameHuman);
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerManager  player = other.GetComponent<PlayerManager>();
            if (player != null)
                if (player.audioEventNames.Contains(audioEventNameElephant))
                {
                    player.audioEventNames.Add(audioEventNameElephant);
                    return;
                }
            
            NpcManager npcManager = other.GetComponent<NpcManager>();
            if (npcManager != null)
                if (npcManager.audioEventNames.Contains(audioEventNameHuman))
                    npcManager.audioEventNames.Remove(audioEventNameHuman);
        }
    }
}

