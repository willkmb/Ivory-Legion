using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Npc.Marker_Points
{
    public enum MarkerType
    {
        Base,
        Action,
    }

    public enum MarkerNpcTarget
    {
        Human,
        Elephant,
    }
    public class MarkerPointManager : MonoBehaviour
    {
        public static MarkerPointManager instance;
        
        // public List<GameObject> markerPointsHumanActive;
        // public List<GameObject> markerPointsHumanInactive;
        //
        // public List<GameObject> markerPointsElephantActive;
        // public List<GameObject> markerPointsElephantInactive;
        private void Awake()
        {
            instance ??= this;
        }

        private void Start()
        {
           Invoke("StartNpcStates", 0.1f);
        }
        // Required to ensure all marker points are attached to the manager
        private void StartNpcStates()
        {
            NpcEvents.instance.NpcCallAllStates();
        }
    }
}
