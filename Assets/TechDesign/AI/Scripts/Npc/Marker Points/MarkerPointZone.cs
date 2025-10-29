using System.Collections.Generic;
using Npc.Marker_Points;
using UnityEngine;

namespace Ai
{
    public class MarkerPointZone : MonoBehaviour
    {
        public List<GameObject> markerPointsHumanActive = new List<GameObject>();
        public List<GameObject> markerPointsHumanInactive = new List<GameObject>();
        public List<GameObject> markerPointsElephantActive = new List<GameObject>();
        public List<GameObject> markerPointsElephantInactive = new List<GameObject>();
    }
}

