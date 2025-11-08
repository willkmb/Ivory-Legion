using System;
using Npc.Marker_Points;
using UnityEngine;
using UnityEngine.Serialization;

namespace Npc.AI
{
    public class NpcPerformingAction : MonoBehaviour
    {
        [HideInInspector] public float timeOfAction; //Will be the length of the animation being used
        private float _timer;
        public MarkerPoint activeMarkerPoint;

        //Scripts
        private NpcManager _npcManager;
        private void Awake()
        {
            _npcManager = transform.GetComponent<NpcManager>();
            _timer = 0f;
            _doOnce = false;
        }

        public void SubscribeToTimer()
        {
            NpcEvents.instance.NpcCheckArrivalEvent += ActionTimer;
        }

        private bool _doOnce; //purely to stop animation looping
        // Gets called even if an action is occuring, 
        private void ActionTimer()
        {
            _timer += NpcEvents.instance.timerCallValue;
           // Plays animation and gets the duration of that animation
            if (!_doOnce)
            {
                switch (activeMarkerPoint.GetMarkerType())
                {
                    case MarkerType.Base:
                        break;
                    case MarkerType.Action:
                        timeOfAction = activeMarkerPoint.GetTestPS().main.duration;
                        activeMarkerPoint.GetTestPS().Play();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _doOnce = true;
            }
            //When the action is complete stop the animation if required
            if (_timer >= timeOfAction)
            {
                switch (activeMarkerPoint.GetMarkerType())
                {
                    case MarkerType.Base:
                        break;
                    case MarkerType.Action:
                        activeMarkerPoint.GetTestPS().Stop();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //Readds the markerPoint to the active list so other npcs can access it
                switch(activeMarkerPoint.GetMpNpcTarget())
                {
                    case MarkerNpcTarget.Human:
                        if(!_npcManager.markerPointZone.markerPointsHumanActive.Contains(activeMarkerPoint.gameObject))
                              _npcManager.markerPointZone.markerPointsHumanActive.Add(activeMarkerPoint.gameObject);
                        
                        _npcManager.markerPointZone.markerPointsHumanInactive.Remove(activeMarkerPoint.gameObject);
                        break;
                    case MarkerNpcTarget.Elephant:
                        if (!_npcManager.markerPointZone.markerPointsElephantActive.Contains(activeMarkerPoint.gameObject))
                            _npcManager.markerPointZone.markerPointsElephantActive.Add(activeMarkerPoint.gameObject);
                        
                        _npcManager.markerPointZone.markerPointsElephantInactive.Remove(activeMarkerPoint.gameObject);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //Resetting the bool and timer value, and setting the npc back to the walking state to start the process over again
                
                _doOnce = false;
                _timer = 0f;

                NpcEvents.instance.NpcCheckArrivalEvent -= ActionTimer;
                
                //Add more complex thought for the state change here at a later date
                _npcManager.npcState = NpcState.Walking;
                _npcManager.StateChanger();
            }
        }
    }
}
