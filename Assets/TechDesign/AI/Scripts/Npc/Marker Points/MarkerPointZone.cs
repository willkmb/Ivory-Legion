using System;
using System.Collections.Generic;
using InputManager;
using Npc;
using Npc.AI;
using Player;
using UnityEngine;

namespace Ai
{
    public class MarkerPointZone : MonoBehaviour
    {
       [HideInInspector] public List<GameObject> markerPointsHumanActive = new List<GameObject>();
       [HideInInspector] public List<GameObject> markerPointsHumanInactive = new List<GameObject>();
       [HideInInspector]  public List<GameObject> markerPointsElephantActive = new List<GameObject>();
       [HideInInspector]  public List<GameObject> markerPointsElephantInactive = new List<GameObject>()
           ;
       [HideInInspector]  public List<NpcManager> activeElephantsNpcs = new List<NpcManager>();
       [HideInInspector]  public List<NpcManager> activeHumanNpcs = new List<NpcManager>();

       [SerializeField] private bool isStartingZone = false;

       private void Start()
       {
           if (isStartingZone)
               NpcEvents.instance.currentMarkerZone = this;
           else
            Debug.LogError("No Starting Zone Selected");
       }
       
       private void OnTriggerEnter(Collider other)
       {
           if (NpcEvents.instance.currentMarkerZone == this) 
            return;
           
           PlayerManager player = other.GetComponent<PlayerManager>();
           if (player != null)
               NpcEvents.instance.currentMarkerZone = this;
       }

       public void AddToZone(NpcManager npcManager)
       {
           switch (npcManager.npcType)
           {
               case NpcType.Humanoid:
                   activeHumanNpcs.Add(npcManager);
                   break;
               case NpcType.Elephant:
                   activeElephantsNpcs.Add(npcManager);
                   break;
               default:
                   throw new ArgumentOutOfRangeException();
           }
       }
    }
}

