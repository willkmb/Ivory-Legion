using System;
using System.Linq;
using Ai;
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
            if (_npcManager.markerPointZone.markerPointsHumanActive.Count <= 0)
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
                    var randomPointHuman = Random.Range(0,  _npcManager.markerPointZone.markerPointsHumanActive.Count - 1);
                     pointObj = _npcManager.markerPointZone.markerPointsHumanActive[randomPointHuman];
                    RemoveMarkerPoint(pointObj);
                    break;
                case NpcType.Elephant:
                    var randomPointElephant = Random.Range(0, _npcManager.markerPointZone.markerPointsElephantActive.Count - 1);
                     pointObj = _npcManager.markerPointZone.markerPointsElephantActive[randomPointElephant];
                    RemoveMarkerPoint(pointObj);
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
                        SetLocation();
                        return;
                    } 
                    break;
                case NpcType.Elephant:
                    if (!NavMesh.CalculatePath(transform.position,pointObj.transform.position, _npcManager.agent.areaMask, _npcWalking.Path))
                    {
                        SetLocation();
                        return;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                    _npcManager.markerPointZone.markerPointsHumanActive.Remove(markerPoint); // Prevents other npcs going to the same location
                    _npcManager.markerPointZone.markerPointsHumanInactive.Add(markerPoint);
                    break;
                case NpcType.Elephant:
                    _npcManager.markerPointZone.markerPointsElephantActive.Remove(markerPoint); // Prevents other npcs going to the same location
                    _npcManager.markerPointZone.markerPointsElephantInactive.Add(markerPoint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
