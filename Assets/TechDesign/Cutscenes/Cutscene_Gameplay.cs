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
        
        
        [Header("Npcs to move during cutscene")]
        [SerializeField] private List<NpcManager> gameObjects = new List<NpcManager>();

        private void Awake()
        {
            mainCam =  Camera.main;
        }

        private bool _doOnce = false;
        private void OnTriggerEnter(Collider other)
        {
            if (_doOnce)
               return; 
            
     
            PlayerManager player = other.transform.parent.GetComponent<PlayerManager>();
             if (player == null) 
                 return;

             foreach (var VARIABLE in gameObjects)
             {
                 VARIABLE.npcState = NpcState.SetPathingWalking;
                 VARIABLE.StateChanger();
             }
            
             mainCam.enabled = false;
             animation.Play();
             _doOnce = true;
             PlayerManager.instance.inCutscene = true;
             
            Invoke("StopAnimation", animation.clip.length);
        }
        private void StopAnimation()
        {
            mainCam.enabled = true;
            PlayerManager.instance.inCutscene = false;
            animation.Stop();
            foreach (var manager in gameObjects)
                manager.gameObject.SetActive(false);

            switch (afterCutscene)
            {
                case CutSceneActivation.Trigger:
                    Debug.Log("Start dialogue with parents");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            gameObject.SetActive(false);
        }
    }
}

