using System;
using System.Collections.Generic;
using InputManager;
using Npc;
using Npc.AI;
using Npc.AI.Movement;
using UnityEngine;

namespace Cutscene
{
    public enum CutSceneActivation
    {
        Trigger,
    }
    public class Cutscene_Gameplay : MonoBehaviour
    {
        public CutSceneActivation activation;
        
        [Header("Npcs to move during cutscene")]
        [SerializeField] private List<NpcManager> npcManagers = new List<NpcManager>();

        private void OnTriggerEnter(Collider other)
        {
           PlayerManager player = other.GetComponent<PlayerManager>();
           if (player == null) 
                return;
           
           foreach (var manager in npcManagers)
           {
               manager.gameObject.SetActive(true);
               manager.npcState = NpcState.SetPathingWalking; // Idle -> SetPathingWalking
               manager.StateChanger();
           }
           gameObject.SetActive(false);
        }
    }
}

