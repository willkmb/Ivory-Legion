using System;
using Npc.Marker_Points;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Npc.AI.Movement
{
    public class NpcWalking : MonoBehaviour
    {
        public NavMeshPath Path;
        public Vector3 targetPos;

        private NavMeshAgent _agent;
        private NpcManager _npcManager;
        
        //scripts
        private NpcPerformingAction _npcPerformingAction;
        private void Start()
        {
            Path = new NavMeshPath();

            _agent = transform.GetComponent<NavMeshAgent>();
            _npcManager = transform.GetComponent<NpcManager>();
            _npcPerformingAction = transform.GetComponent<NpcPerformingAction>();
        }

        private bool _requiresAction;
        private GameObject markerpoint;
        public void WalkToLocation(Vector3 pos, MarkerPoint markerPoint)
        {
            _requiresAction = markerPoint.RequiresAction();
            _npcPerformingAction.activeMarkerPoint = markerPoint;
            markerpoint = markerPoint.transform.gameObject;

            targetPos = pos;
            _agent.SetPath(Path);
        }

        public void ArrivalChecker()
        { 
            //If not in the walking state, return
            if (_npcManager.npcState != NpcState.Walking)
                return;
            
            if (Math.Abs(_agent.transform.position.x - targetPos.x) < 0.1f
                && Math.Abs(_agent.transform.position.z - targetPos.z) < 0.1f)
            {
                if(_requiresAction)
                    _npcManager.npcState = NpcState.PerformingAction;
                if(!_requiresAction)
                    _npcManager.npcState = NpcState.Idle;
                
                _npcManager.minMovementCooldownTime = _npcPerformingAction.activeMarkerPoint.GetMinMovementCooldownTime();
                _npcManager.maxMovementCooldownTime = _npcPerformingAction.activeMarkerPoint.GetMaxMovementCooldownTime();

                _npcManager.markerPointZone.markerPointsHumanActive.Add(markerpoint);

                NpcEvents.instance.NpcCheckArrivalEvent -= ArrivalChecker;
                _npcManager.StateChanger();
            }
        }
    }
}
