using System;
using System.Collections;
using System.Collections.Generic;
using InputManager;
using Npc;
using Npc.AI;
using Npc.AI.Movement;
using UnityEngine;
using UnityEngine.AI;

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
        private float time = 0f;
        
        [Header("Timers")]
        [SerializeField] private float parentWalkTimer;
        [SerializeField] private float humanWalkTimer;
        
        [Header("Npcs to move during cutscene")]
        [SerializeField] private List<NpcManager> walkingElephants = new List<NpcManager>();
        [SerializeField] private List<NpcManager> parents = new List<NpcManager>();
        [SerializeField] private List<NpcManager> humans = new List<NpcManager>();
        [SerializeField] private GameObject blocker;

        [SerializeField] private GameObject parent;
        [SerializeField] public static bool finishedParent = false;

        private void Awake()
        {
            mainCam =  Camera.main;
            _doOnce = false;
            blocker.SetActive(false);
        }

        private void Start()
        {
            parent.GetComponent<PromptScript>().thisPrompt.SetActive(false);
            parent.GetComponent<Dialogue>().enabled = false;
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
                float dis = Vector3.Distance(VARIABLE.transform.position, VARIABLE.GetComponent<NpcSetPathWalking>().movementLocations[1]);
                float speed = VARIABLE.GetComponent<NavMeshAgent>().speed;
                time = dis/ speed;
                VARIABLE.npcState = NpcState.SetPathingWalking;
                VARIABLE.StateChanger();

                //StartCoroutine(PauseParentsForDialogue( VARIABLE, time));
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
                    parent.GetComponent<PromptScript>().thisPrompt.SetActive(true);
                    parent.GetComponent<Dialogue>().enabled = true;
                    resetHandler();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            blocker.SetActive(true);
            gameObject.SetActive(false);
        }

        public void resetHandler()
        {
            if (finishedParent)
            {
                GameObject handler = GameObject.Find("Handler_AI_NPC");
                handler.GetComponentInParent<PromptScript>().thisPrompt.SetActive(true);
                handler.GetComponentInParent<Dialogue>().enabled = true;
            }
        }

        /*private IEnumerator PauseParentsForDialogue(NpcManager VARIABLE,float timeLocal)
        {
            yield return new WaitForSeconds(timeLocal);
            float dis = Vector3.Distance(VARIABLE.transform.position, VARIABLE.GetComponent<NpcSetPathWalking>().movementLocations[1]);
            if (dis < 0.25f) { VARIABLE.npcState = NpcState.Idle; VARIABLE.StateChanger(); }
        }*/
    }
}

