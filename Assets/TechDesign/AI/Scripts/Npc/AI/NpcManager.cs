using System;
using System.Collections.Generic;
using Ai;
using Npc.AI.Movement;
using Audio;
using Quests;
using UnityEngine;
using UnityEngine.AI;
using static Interfaces.Interfaces;
using Random = UnityEngine.Random;

namespace Npc.AI
{
    public enum NpcState
    {
        Idle, //If alwaysIdle ticked never move, otherwise this is used as part of the performingAction function(s) process
        Walking, //Walks to an available Marker Point
        SetPathingWalking, //Goes to set locations, loops around
        PerformingAction, // Certain marker points requires actions (E.G Animations) to be performed before choosing another maker point to move to
        AvoidingPlayer, // N/A
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

        //Components
        [HideInInspector] public NavMeshAgent agent;
        
        //Scripts
        private NpcSetLocation _npcSetLocation;
        private NpcPerformingAction _performingAction;
        [HideInInspector] public NpcSetPathWalking setPathWalking;
        private NpcRandomMovement _randomMovement;
        private Dialogue _dialogue;
        
        // Values
        private float _agentOriginalSpeed;
        [Header("Make the AI stay still")]
        public bool alwaysIdle; // Ticked if you want the NPC to always stay in the same location (E.G Shop Merchant)
        [Header("Variables")]
        public bool patrolling; // Ticked if you want the NPC to travel between points on the SetPathingWalking
        [HideInInspector] public float minMovementCooldownTime;
        [HideInInspector] public float maxMovementCooldownTime;
        public bool usedInCutscene;
        
        [Header("Only Required if AI is 'BASE'")]
        public MarkerPointZone markerPointZone;
        
        //Quests
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
        }

        private void Start()
        {
            NpcEvents.instance.NpcCallAllStatesEvent += StateMachine;

            if (alwaysIdle)
                npcState = NpcState.Idle;
        }

        // The brain of the NPC
        private void StateMachine()
        {
            switch (npcState)
            {
                case NpcState.Idle: //NPC will not move,
                    agent.speed = 0f;
                    stateSaver = NpcState.Idle;
                    if (!alwaysIdle)
                    {
                        var randomTime = Random.Range(minMovementCooldownTime, maxMovementCooldownTime);
                        _performingAction.timeOfAction = randomTime;
                        _performingAction.SubscribeToTimer();
                    }
                    break;
                case NpcState.Walking: // NPC walks to set location(s), set by parameters
                    stateSaver = NpcState.Walking;
                    _npcSetLocation.SetLocation(); // Gets location for the npc to walk too
                    break;
                case NpcState.SetPathingWalking: //Selected for Npcs that walk to point A TO B with no other objective
                    // Only needs to be called once as it loops itself in a contained script
                    stateSaver =  NpcState.SetPathingWalking;
                    setPathWalking.GetNextLocationPoint();
                    break;
                case NpcState.PerformingAction: //E.G Animations involving jobs or trading with the shop owner
                    stateSaver  =  NpcState.PerformingAction;
                    _performingAction.SubscribeToTimer();
                    break;
                case NpcState.AvoidingPlayer: // When the NPC is near the player, move backwards / away to avoid collision with the player.
                    //Might not need this still testing
                    break;
                case NpcState.TalkingToPlayer: //NPC will stop any movement and enter the dialogue with the player
                    agent.speed = 0f;
                    _dialogue.pastNpcState = stateSaver;
                    // Quests // - Alters Text based of quest completion state
                    if (questDialogueAlterer != null)
                        questDialogueAlterer.ChangeDialogueBasedOnQuests();
                    break;
                case NpcState.RandomPathing: //Walk to a random spot with x radius to simulate that they are busy.
                    //Repeats in an infinite loop unless spoken to by player in which it will continue after the conversation
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
    }
}

