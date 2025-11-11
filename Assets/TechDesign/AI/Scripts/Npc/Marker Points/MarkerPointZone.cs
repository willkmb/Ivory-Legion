using System.Collections.Generic;
using Npc.Marker_Points;
using UnityEngine;

namespace Ai
{
    public class MarkerPointZone : MonoBehaviour
    {
       [HideInInspector] public List<GameObject> markerPointsHumanActive = new List<GameObject>();
       [HideInInspector] public List<GameObject> markerPointsHumanInactive = new List<GameObject>();
       [HideInInspector]  public List<GameObject> markerPointsElephantActive = new List<GameObject>();
       [HideInInspector]  public List<GameObject> markerPointsElephantInactive = new List<GameObject>();
    }
}

