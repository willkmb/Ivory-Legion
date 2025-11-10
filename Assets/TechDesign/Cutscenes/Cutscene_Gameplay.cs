using System;
using System.Collections.Generic;
using InputManager;
using Npc;
using Npc.AI;
using Npc.AI.Movement;
using UnityEngine;

namespace Cutscene
{
    public enum CutSceneActivation
    {
        Trigger,
    }

    public enum AfterCutscene
    {
        Dialogue,
    }
    public class Cutscene_Gameplay : MonoBehaviour
    {
        public CutSceneActivation activation;
        public CutSceneActivation afterCutscene;
        [SerializeField] private new Animation animation;
        [SerializeField] private Camera mainCam;
        [SerializeField] private Camera animationCam;
        
        [Header("Timers")]
        [SerializeField] private float parentWalkTimer;
        [SerializeField] private float humanWalkTimer;
        
        [Header("Npcs to move during cutscene")]
        [SerializeField] private List<NpcManager> walkingElephants = new List<NpcManager>();
        [SerializeField] private List<NpcManager> parents = new List<NpcManager>();
        [SerializeField] private List<NpcManager> humans = new List<NpcManager>();
        [SerializeField] private GameObject blocker;

        private void Awake()
        {
            mainCam =  Camera.main;
            _doOnce = false;
            blocker.SetActive(false);
        }

        private bool _doOnce;
        private void OnTriggerEnter(Collider other)
        {
            if (_doOnce)
               return; 
            
     
            PlayerManager player = other.transform.parent.GetComponent<PlayerManager>();
             if (player == null) 
                 return;

             foreach (var VARIABLE in walkingElephants)
             {
                 VARIABLE.npcState = NpcState.SetPathingWalking;
                 VARIABLE.StateChanger();
             }
            
             mainCam.enabled = false;
             animation.Play();
             _doOnce = true;
             PlayerManager.instance.inCutscene = true;
             
             
            Invoke("ParentsWalk", parentWalkTimer); 
            Invoke("HumanWalk", humanWalkTimer); 
            Invoke("StopAnimation", animation.clip.length);
        }

        private void ParentsWalk()
        {
            foreach (var VARIABLE in parents)
            {
                VARIABLE.npcState = NpcState.SetPathingWalking;
                VARIABLE.StateChanger();
            }
        }
        private void HumanWalk()
        {
            foreach (var VARIABLE in humans)
            {
                VARIABLE.npcState = NpcState.SetPathingWalking;
                VARIABLE.StateChanger();
            }
        }
        private void StopAnimation()
        {
            mainCam.enabled = true;
            PlayerManager.instance.inCutscene = false;
            animation.Stop();
            foreach (var manager in walkingElephants)
                manager.gameObject.SetActive(false);

            switch (afterCutscene)
            {
                case CutSceneActivation.Trigger:
                    Debug.Log("Start dialogue with parents");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            blocker.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

