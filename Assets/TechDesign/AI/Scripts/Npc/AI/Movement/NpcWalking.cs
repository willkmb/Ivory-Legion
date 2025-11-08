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

        private MarkerPoint _oldMp;
        
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
            _npcManager.currentMovPos = targetPos;
            
            _agent.SetPath(Path);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void ArrivalChecker()
        { 
            //If not in the walking state, return
            if (_npcManager.npcState != NpcState.Walking)
                return;
            
            if (Math.Abs(_agent.transform.position.x - _npcManager.currentMovPos.x) < 0.1f
                && Math.Abs(_agent.transform.position.z - _npcManager.currentMovPos.z) < 0.1f)
            {
                float delay = markerpoint.transform.GetComponent<MarkerPoint>().RandomDelay();

                if (_requiresAction)
                {
                    _npcManager.npcState = NpcState.PerformingAction;
                    delay = markerpoint.transform.GetComponent<MarkerPoint>().GetTestPS().time 
                            + (markerpoint.transform.GetComponent<MarkerPoint>().GetTestPS().time * 0.1f);
                }
                if(!_requiresAction)
                    _npcManager.npcState = NpcState.Idle;
                
                _npcManager.minMovementCooldownTime = _npcPerformingAction.activeMarkerPoint.GetMinMovementCooldownTime();
                _npcManager.maxMovementCooldownTime = _npcPerformingAction.activeMarkerPoint.GetMaxMovementCooldownTime();
                
                _oldMp = markerpoint.GetComponent<MarkerPoint>();
                Invoke("ReAddMarkerPoint", delay);
                
                NpcEvents.instance.NpcCheckArrivalEvent -= ArrivalChecker;
                _npcManager.StateChanger();
            }
        }

        private void ReAddMarkerPoint()
        {
            switch(_oldMp.GetMpNpcTarget())
            {
                case MarkerNpcTarget.Human:
                    if(!_npcManager.markerPointZone.markerPointsHumanActive.Contains(_oldMp.gameObject))
                        _npcManager.markerPointZone.markerPointsHumanActive.Add(_oldMp.gameObject);
                        
                    _npcManager.markerPointZone.markerPointsHumanInactive.Remove(_oldMp.gameObject);
                    break;
                case MarkerNpcTarget.Elephant:
                    if (!_npcManager.markerPointZone.markerPointsElephantActive.Contains(_oldMp.gameObject))
                        _npcManager.markerPointZone.markerPointsElephantActive.Add(_oldMp.gameObject);
                        
                    _npcManager.markerPointZone.markerPointsElephantInactive.Remove(_oldMp.gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
