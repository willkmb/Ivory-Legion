using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Npc.AI.Movement
{
    public class NpcSetPathWalking : MonoBehaviour
    {
        //Lists
        [Header("Ensure locations are on navmesh")]
        [SerializeField] private List<Vector3> movementLocations;
        [SerializeField] private List<Material> skinList;
       
        //Components 
        private NavMeshPath _path;
        
        // Scrips
        private NpcManager _npcManager;

        //Values
        private Vector3 _targetPos;
        [FormerlySerializedAs("_currentPointNumber")] 
        [HideInInspector] public int currentPointNumber;
        private int _skinNumber;
        private float _resetTimer;
        private bool _resetting;
        
        //Variables
        [SerializeField] private float timeToReset;
        
        private void Awake()
        {
            _path = new NavMeshPath();
            
            _npcManager = transform.GetComponent<NpcManager>();
            
            currentPointNumber = 0;
            if (_npcManager.usedInCutscene)
                currentPointNumber = movementLocations.Count - 1;
            
            _resetTimer = 0;
        }

        private void Start()
        {
            NpcEvents.instance.NpcCheckArrivalEvent += ArrivalChecker;
            
            movementLocations[0] = transform.position;
            
            if (_npcManager.usedInCutscene)
                gameObject.SetActive(false);
        }

        public void GetNextLocationPoint()
        {
            if (_resetting)
                return;
            
            //If all marker points have been reached and the AI is not patrolling
            if (movementLocations.Count <= currentPointNumber && !_npcManager.patrolling)
            {
                // Cutscene deactivation
                if (_npcManager.usedInCutscene)
                {
                    _npcManager.npcState = NpcState.Idle;
                    gameObject.SetActive(false);
                    return;
                }
                
                // Do reset Stuff - Different skin
                _resetting = true;
                NpcEvents.instance.NpcCheckArrivalEvent += ResetTimer;
                ChangeSkin();
                return;
            }

            //Called if ai is patrolling in this state - Prevents them from teleporting to start they walk between them
            if (_npcManager.patrolling)
                currentPointNumber = 0;
            
            _targetPos = movementLocations[currentPointNumber];
            MoveToLocation();
        }
        
        private void MoveToLocation()
        {
            //Called if ai IS patrolling
            if (_npcManager.patrolling)
            {
                var currentLocation = movementLocations[0];
                movementLocations.Remove(currentLocation);
                movementLocations.Add(currentLocation);
            }
            else //Below is only called if the ai is NOT patrolling
                currentPointNumber += 1;
            
            switch (_npcManager.npcType)
            {
                case NpcType.Humanoid:
                    NavMesh.CalculatePath(transform.position, _targetPos, NavMesh.AllAreas, _path);
                    break;
                case NpcType.Elephant:
                    NavMesh.CalculatePath(transform.position,_targetPos, _npcManager.agent.areaMask, _path);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _npcManager.agent.SetPath(_path);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void ArrivalChecker()
        {
            if (Math.Abs(_npcManager.agent.transform.position.x - _targetPos.x) < 0.1f
                && Math.Abs(_npcManager.agent.transform.position.z - _targetPos.z) < 0.1f)
            {
                GetNextLocationPoint();
            }
        }
        //Called every time the location rotation resets 
        private void ChangeSkin()
        {
            if (skinList.Count <= _skinNumber)
                _skinNumber = 0;
            
            transform.gameObject.GetComponent<Renderer>().material = skinList[_skinNumber];
            _skinNumber += 1;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void ResetTimer()
        {
           _resetTimer += NpcEvents.instance.timerCallValue; 
           
            if (_resetTimer >= timeToReset)
            {
               // Teleports object back to the starting point
                NpcEvents.instance.NpcCheckArrivalEvent -= ResetTimer;
                _resetTimer = 0f;
                currentPointNumber = 0; 
                transform.localPosition = movementLocations[0];
                _resetting = false;
                GetNextLocationPoint();
            }
        }
    }
}
