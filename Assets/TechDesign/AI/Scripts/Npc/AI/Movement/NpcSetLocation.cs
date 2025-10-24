using System;
using System.Linq;
using Npc.Marker_Points;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Npc.AI.Movement
{
    public class NpcSetLocation : MonoBehaviour
    {
        private NpcWalking _npcWalking;
        private NpcManager _npcManager;

        private void Awake()
        {
            _npcWalking = transform.GetComponent<NpcWalking>();
            _npcManager = transform.GetComponent<NpcManager>();
        }
        public void SetLocation()
        {
            //If there is no active points to move towards return to idle state
            if (MarkerPointManager.instance.markerPointsHumanActive.Count <= 0)
            {
                _npcManager.npcState = NpcState.Idle;
                _npcManager.StateChanger();
                return;
            }
            //If there are marker points to more too, select a random active one
            GameObject pointObj;
            switch (_npcManager.npcType)
            {
                case NpcType.Humanoid:
                    var mpAmountHuman = MarkerPointManager.instance.markerPointsHumanActive.Count;
                    var randomPointHuman = Random.Range(0, mpAmountHuman);
                     pointObj = MarkerPointManager.instance.markerPointsHumanActive[randomPointHuman];
                    break;
                case NpcType.Elephant:
                    var mpAmountElephant = MarkerPointManager.instance.markerPointsElephantActive.Count;
                    var randomPointElephant = Random.Range(0, mpAmountElephant);
                     pointObj = MarkerPointManager.instance.markerPointsElephantActive[randomPointElephant];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // Checks to see if the npc can arriving to location safely and if it can't remove from list of marker options
            // Sets the value of the path
            switch (_npcManager.npcType)
            {
                case NpcType.Humanoid:
                    if (!NavMesh.CalculatePath(transform.position,pointObj.transform.position, NavMesh.AllAreas, _npcWalking.Path))
                    {
                        RemoveMarkerPoint(pointObj);
                        SetLocation();
                        return;
                    } 
                    break;
                case NpcType.Elephant:
                    if (!NavMesh.CalculatePath(transform.position,pointObj.transform.position, _npcManager.agent.areaMask, _npcWalking.Path))
                    {
                        RemoveMarkerPoint(pointObj);
                        SetLocation();
                        return;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
          
            //Called if npc can arrive at marker point
            RemoveMarkerPoint(pointObj); // Prevents other npcs going to the same location
            
            var position = pointObj.transform.position;
            _npcWalking.targetPos = position;

            // Checks if an action is required at this location , effecting the state (E.G Fishing or talking to a merchant)
            MarkerPoint markerPoint = pointObj.transform.GetComponent<MarkerPoint>();
            
            NpcEvents.instance.NpcCheckArrivalEvent += _npcWalking.ArrivalChecker;
            _npcWalking.WalkToLocation(position, markerPoint);
        }

        private void RemoveMarkerPoint(GameObject markerPoint)
        {
            switch(_npcManager.npcType)
            {
                case NpcType.Humanoid:
                    MarkerPointManager.instance.markerPointsHumanActive.Remove(markerPoint); // Prevents other npcs going to the same location
                    MarkerPointManager.instance.markerPointsHumanInactive.Add(markerPoint);
                    break;
                case NpcType.Elephant:
                    MarkerPointManager.instance.markerPointsElephantActive.Remove(markerPoint); // Prevents other npcs going to the same location
                    MarkerPointManager.instance.markerPointsElephantInactive.Add(markerPoint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
