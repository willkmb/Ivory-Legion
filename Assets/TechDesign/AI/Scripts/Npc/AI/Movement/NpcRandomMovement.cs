using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Npc.AI.Movement
{
    public class NpcRandomMovement : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField] private float radius;
        [SerializeField] LayerMask layerMask;
        [Header("FALSE = Start Pos, TRUE = Current Pos")]
        [SerializeField] private bool radiusOnSelf;
        private Vector3 _radiusPosition;
        [Range(0f, 25f)] [SerializeField] private float cooldown;
        
        //Scripts
        private NpcManager _npcManager;
        
        private NavMeshPath _path;
        private void Start()
        {
            _npcManager = transform.GetComponent<NpcManager>();
            _path = new NavMeshPath();
            
            if (radiusOnSelf) //Sets the location of the search circle to the transforms START position
                _radiusPosition = transform.position;
        }

        // Gets random location within the sphere 
        public void GetRandomlocation()
        {
            if (!radiusOnSelf) //Sets the location of the search circle to the transforms CURRENT position
                _radiusPosition = transform.position;
            
            var randomPoint = Random.insideUnitSphere * radius + _radiusPosition;
            FireRayCastYAxis(randomPoint); // Firing Raycast to check if randomPoint is on navmesh
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private void FireRayCastYAxis(Vector3 randomPoint)
        {
            //Fires a raycast to get the Y level of the location, this is so the AI doesn't try to fly into the sky to get to the location
            RaycastHit hit;
            randomPoint.y += 50f;
            if (Physics.Raycast(randomPoint, Vector3.down, out hit, 100f, layerMask))
            {
                if(LocationNavmeshCheck(hit.point)) //Checks if the location in on the navmesh
                { // If it is moving to that location
                    Movement();
                    return;
                }
                // If not on the navmesh, try again with new randomPoint
            }
            GetRandomlocation();
        }
        //Checks if the location is on the navmesh
        private bool LocationNavmeshCheck(Vector3 randomPoint)
        {
            NavMeshHit hit;
            switch (_npcManager.npcType)
            {
                case NpcType.Humanoid:
                    if (NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
                    {
                        _movePos = hit.position;
                        return Vector3.Distance(randomPoint, _movePos) <= 0.6f;
                    }
                    break;
                case NpcType.Elephant:
                    if (NavMesh.SamplePosition(randomPoint, out hit, 1, _npcManager.agent.areaMask))
                    {
                        _movePos = hit.position;
                        return Vector3.Distance(randomPoint, _movePos) <= 0.6f;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }
        // Moves the Npc to set location and subscribes to the arrival checker
        private void Movement()
        {
            NpcEvents.instance.NpcCheckArrivalEvent += ArrivalChecker;
            switch (_npcManager.npcType)
            {
                case NpcType.Humanoid:
                    NavMesh.CalculatePath(transform.position, _movePos, NavMesh.AllAreas, _path);
                    break;
                case NpcType.Elephant:
                    NavMesh.CalculatePath(transform.position,_movePos, _npcManager.agent.areaMask, _path);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _npcManager.currentMovPos = _movePos;
            
            _npcManager.agent.SetPath(_path);
        }

        private Vector3 _movePos;
        // Checks when the Npc has arrived at its location, when it has start the loop again.
        private void ArrivalChecker()
        { 
            if (Math.Abs(_npcManager.agent.transform.position.x - _movePos.x) < 0.1f
                && Math.Abs(_npcManager.agent.transform.position.z - _movePos.z) < 0.1f && Math.Abs((_npcManager.agent.transform.position.y - _movePos.y)) < 1.5f)
            {
                NpcEvents.instance.NpcCheckArrivalEvent -= ArrivalChecker;
                Invoke("GetRandomlocation", cooldown);
            }
        }
    }
}
