using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ai;
using Npc.AI.Movement;
using Audio;
using InputManager;
using Quests;
using UnityEngine;
using UnityEngine.AI;
using static Interfaces.Interfaces;
using Random = UnityEngine.Random;

namespace Npc.AI
{
    
    public enum NpcState // Data that never change
    {
        Idle, //If alwaysIdle ticked never move, otherwise this is used as part of the performingAction function(s) process
        Walking, //Walks to an available Marker Point
        SetPathingWalking, //Goes to set locations, loops around
        PerformingAction, // Certain marker points requires actions (E.G Animations) to be performed before choosing another maker point to move to
        TalkingToPlayer, // Stops moving and talks to player
        RandomPathing, //Chooses a random point within x radius of the AI
    }

    public enum NpcType
    {
        Humanoid,
        Elephant
    }
    public class NpcManager : MonoBehaviour , INpc
    {
        //Enums
        [Header("Variables")]
        public NpcType npcType;
        public NpcState npcState;
        [HideInInspector] public NpcState stateSaver;

        // Components
        [HideInInspector] public NavMeshAgent agent;
        private GameObject _blocker;
        
        // Scripts
        private NpcSetLocation _npcSetLocation;
        private NpcPerformingAction _performingAction;
        [HideInInspector] public NpcSetPathWalking setPathWalking;
        private NpcRandomMovement _randomMovement;
        private Dialogue _dialogue;
        
        // Values
        [Header("Values / Variables")]
        private float _agentOriginalSpeed;
        public bool alwaysIdle; // Ticked if you want the NPC to always stay in the same location (E.G Shop Merchant)
        public bool patrolling; // Ticked if you want the NPC to travel between points on the SetPathingWalking
        [HideInInspector] public float minMovementCooldownTime;
        [HideInInspector] public float maxMovementCooldownTime;
        public bool idleAfterCutscene;
        [HideInInspector] public Vector3 currentMovPos;
        
        [Header("Only Required if AI is 'BASE'")]
        public MarkerPointZone markerPointZone;
        
        // Audio
        [Header("Audio")]
        [HideInInspector] public bool isWalking;
        private float audioTimer;
        [SerializeField] private float audioFrequencyTime;
        [SerializeField] private float playerHearingRange;
        public List<string> audioEventNames = new List<string>();
        
        
        // Quests
        [Header("Here for Testing")]
        public Quest_DialogueAlterer questDialogueAlterer;
   
        private void Awake()
        {
            agent = transform.GetComponent<NavMeshAgent>();
            _agentOriginalSpeed = agent.speed;
            
            _npcSetLocation = transform.GetComponent<NpcSetLocation>();
            _performingAction = transform.GetComponent<NpcPerformingAction>();
            setPathWalking = transform.GetComponent<NpcSetPathWalking>();
            _randomMovement = transform.GetComponent<NpcRandomMovement>();
            _dialogue = transform.GetComponent<Dialogue>();

            audioFrequencyTime =
                Mathf.Clamp(audioFrequencyTime, audioFrequencyTime *0.75f, audioFrequencyTime *1.25f);
        }

        private void Start()
        {
            NpcEvents.instance.NpcCallAllStatesEvent += StateMachine;

            if (alwaysIdle)
                npcState = NpcState.Idle;
            
            markerPointZone.AddToZone(this);
        }
        private void Update()
        {
            // Calls audio for Npcs
            if (NpcEvents.instance.currentMarkerZone.activeElephantsNpcs.Contains(this) ||  NpcEvents.instance.currentMarkerZone.activeHumanNpcs.Contains(this))
            {
                audioTimer += Time.deltaTime;
                if (audioTimer > audioFrequencyTime && isWalking)
                {
                    audioTimer = 0f;
                    NpcWalkingAudio();
                }
            }
        }
        // The brain of the NPC
        private void StateMachine()
        {
            switch (npcState)
            {
                case NpcState.Idle: //NPC will not move,
                    isWalking = false;
                    agent.speed = 0f;
                    stateSaver = NpcState.Idle;
                    _blocker = NpcEvents.instance.GetBlocker(); NpcEvents.instance.SetBlocker(transform.position, _blocker);
                    if (!alwaysIdle)
                    {
                        var randomTime = Random.Range(minMovementCooldownTime, maxMovementCooldownTime);
                        _performingAction.timeOfAction = randomTime;
                        _performingAction.SubscribeToTimer();
                    }
                    break;
                case NpcState.Walking: // NPC walks to set location(s), set by parameters
                    if (_blocker != null)
                        NpcEvents.instance.ResetBlocker(_blocker);
                    isWalking = true;
                    stateSaver = NpcState.Walking;
                    _npcSetLocation.SetLocation(); // Gets location for the npc to walk too
                    break;
                case NpcState.SetPathingWalking: //Selected for Npcs that walk to point A TO B with no other objective
                    // Only needs to be called once as it loops itself in a contained script
                    if (_blocker != null)
                        NpcEvents.instance.ResetBlocker(_blocker);
                    isWalking = true;
                    stateSaver =  NpcState.SetPathingWalking;
                    setPathWalking.GetNextLocationPoint();
                    break;
                case NpcState.PerformingAction: //E.G Animations involving jobs or trading with the shop owner
                    isWalking = false;
                    stateSaver  =  NpcState.PerformingAction;
                    _blocker = NpcEvents.instance.GetBlocker(); NpcEvents.instance.SetBlocker(transform.position, _blocker);
                    _performingAction.SubscribeToTimer();
                    break;
                case NpcState.TalkingToPlayer: //NPC will stop any movement and enter the dialogue with the player
                    isWalking = false;
                    agent.speed = 0f;
                    _dialogue.pastNpcState = stateSaver;
                    // Quests // - Alters Text based of quest completion state
                    _blocker = NpcEvents.instance.GetBlocker(); NpcEvents.instance.SetBlocker(transform.position, _blocker);
                    if (questDialogueAlterer != null)
                        questDialogueAlterer.ChangeDialogueBasedOnQuests();
                    break;
                case NpcState.RandomPathing: //Walk to a random spot with x radius to simulate that they are busy.
                    //Repeats in an infinite loop unless spoken to by player in which it will continue after the conversation
                    if (_blocker != null)
                         NpcEvents.instance.ResetBlocker(_blocker);
                    isWalking = true;
                    stateSaver =  NpcState.RandomPathing;
                    _randomMovement.GetRandomlocation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis - Ignore This
        // Called whenever the state is changed
        public void StateChanger()
        {
            agent.speed = _agentOriginalSpeed;
            StateMachine();
        }
        
        private void NpcWalkingAudio()
        {
            // Add zone stuff to help optimise the game
            float distance = Vector3.Distance(transform.position, PlayerManager.instance.transform.position);
            if (playerHearingRange > distance && NpcEvents.instance.currentNumberOfAudioPlayers < NpcEvents.instance.maxNumberOfAudioPlayers)
            {
                NpcEvents.instance.currentNumberOfAudioPlayers += 1;
                    // If npc is in a terrain area, play that audio instead
                    if (audioEventNames.Count >= 1)
                    {
                        AudioManager.instance.PlayFMODSound(transform.position, audioEventNames[0], 1f, true, true, false,
                            true,false, 0.9f, 1.1f, 
                            true, 0.9f, 1.1f, 
                            true, 
                            false, null, null);
                        StartCoroutine(ResetAudio(1));
                        return;
                    }
                    
                    // Plays base Audio if there is no terrain audio
                    AudioManager.instance.PlayFMODSound(transform.position, "event:/SFX/Walking/Humans/H_Walking_Base", 1f, true, true, false,
                        true,true, 0.9f, 1.1f,
                        true, 0.9f, 1.1f, 
                        true, 
                        false, null, null);
                    StartCoroutine(ResetAudio(1));
            }
        }
       

        IEnumerator ResetAudio(int secs)
        {
            yield return new WaitForSeconds(secs);
            NpcEvents.instance.currentNumberOfAudioPlayers -= 1;

            audioFrequencyTime = Random.Range(audioFrequencyTime * 0.75f, audioFrequencyTime * 1.25f);
        }
    }
}

