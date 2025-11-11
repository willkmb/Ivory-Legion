using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Npc
{
    public class NpcEvents : MonoBehaviour
    {
        public static NpcEvents instance;
        [HideInInspector] public float timerCallValue;

        [Header("Pool Values")]
        [SerializeField] private GameObject blocker;
        public List<GameObject> _freeBlockerList;
        
        [Header("Agent Values")] 
        [SerializeField] private float avoidancePredictionTime;
        [SerializeField] private int pathFindingIterationsPerFrame;
        
        private void Awake()
        {
            instance ??= this;

            timerCallValue = 0.05f;
        }

        private void Start()
        {
            SpawnPool();
        }

        private float _timer;
        private float _pathingResetTimer;
        private void Update()
        {
            NavMesh.avoidancePredictionTime = avoidancePredictionTime;
            NavMesh.pathfindingIterationsPerFrame = pathFindingIterationsPerFrame;
            
            _timer += Time.deltaTime;
            
            if (_timer > timerCallValue) //Second Number dictates how fast it is called every second E.G 1/0.05 = 20 times per second
            {
                _timer = 0f;
                NpcCheckArrival();
            }
        }

        private void SpawnPool()
        {
            for (int i = 0; i < 100; i++)
            {
                Debug.Log("Spawning blocker");
                GameObject go = Instantiate(blocker, transform.position, Quaternion.identity);
                go.transform.parent = transform;
                _freeBlockerList.Add(go);
                go.SetActive(false);
            }
        }

        public GameObject GetBlocker()
        {
            return _freeBlockerList[Random.Range(0, _freeBlockerList.Count - 1)];
        }
        public void SetBlocker(Vector3 position, GameObject blocker)
        {
            blocker.transform.position = position;
            blocker.SetActive(true);
        }

        public void ResetBlocker(GameObject blocker)
        {
            blocker.SetActive(false);
            _freeBlockerList.Add(blocker);
        }
        
        public event UnityAction NpcCheckArrivalEvent;

        public void NpcCheckArrival()
        {
            NpcCheckArrivalEvent?.Invoke();
        }

        public event UnityAction NpcCallAllStatesEvent;
        public void NpcCallAllStates()
        {
            NpcCallAllStatesEvent?.Invoke();
        }
    }
}
