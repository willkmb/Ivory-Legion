using Npc;
using Npc.AI;
using Player;
using UnityEngine;
using static Interfaces.Interfaces;
namespace Npc.AI.Movement
{
    public class AiTalkingToPlaye : MonoBehaviour
    {
        private GameObject _playerGo;
        private NpcManager _npcManager;
        private NpcState _pastState;

        private void Start()
        {
            _npcManager = transform.parent.GetComponent<NpcManager>();
          //  InputManager.InputManager.instance.IpressEvent += StopTalking;
        }


        private void OnTriggerStay(Collider other)
        {
            PlayerInterface player = other.transform.GetComponent<PlayerInterface>();
            if (player != null)
            {
                InputManager.InputManager.instance.EpressEvent += TalkingToPlayer;
                _playerGo = other.gameObject;
            }
        }
        // Changed state when converstation is over. - When Will's code is sent over 

        //Will require saved state before npc was spoken to
        // If moving to location, make sure it is the same location (positoned saved)
        private void StopTalking()
        {
          //  InputManager.InputManager.instance.EpressEvent -= TalkingToPlayer;

            _npcManager.setPathWalking.currentPointNumber -= 1;
            _npcManager.npcState = _pastState;


            _npcManager.StateChanger();
        }
        private void TalkingToPlayer()
        {
            if (_npcManager != null)
            {
                _npcManager.npcState = NpcState.TalkingToPlayer;
                _npcManager.StateChanger();
            }
        }
    }
}
