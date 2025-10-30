using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Events;
using static Interfaces.Interfaces;

namespace Npc
{
    public class NpcEvents : MonoBehaviour
    {
        public static NpcEvents instance;
        [HideInInspector] public float timerCallValue;
        private void Awake()
        {
            instance ??= this;

            timerCallValue = 0.05f;
        }

        private float _timer;
        private float _pathingResetTimer;
        private void Update()
        {
            _timer += Time.deltaTime;
            
            if (_timer > timerCallValue) //Second Number dictates how fast it is called every second E.G 1/0.05 = 20 times per second
            {
                _timer = 0f;

                NpcCheckArrival();
            }
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
