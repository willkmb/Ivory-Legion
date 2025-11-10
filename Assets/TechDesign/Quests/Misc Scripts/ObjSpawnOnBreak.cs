using System;
using InputManager;
using UnityEngine;

namespace Mechanic_Destruction
{
    public enum OnDestructionType
    {
        Instantiate,
        EnableObj
    }

    public enum ObjType // Add later
    {
        ID,
        Obj, 
    }

    public enum OnBreak
    {
        Null,
        StartDialogue,
    }
    public class ObjSpawnOnBreak : MonoBehaviour
    {
        [SerializeField] private GameObject spawnObj;
        public OnDestructionType onDestruction;
        public OnBreak onBreak;

        private void Start()
        {
            if (onDestruction == OnDestructionType.EnableObj)
                spawnObj.SetActive(false);
        }

        public void SpawnObj()
        {
            switch (onDestruction)
            {
                case OnDestructionType.Instantiate:
                    GameObject go = Instantiate(spawnObj, transform.position, Quaternion.identity);
                    break;
                case OnDestructionType.EnableObj:
                    spawnObj.SetActive(true);
                    spawnObj.transform.position = transform.position;
                    spawnObj.transform.LookAt(PlayerManager.instance.transform.position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (onBreak)
            {
                case OnBreak.Null:
                    break;
                case OnBreak.StartDialogue:
                    // Start dialogue
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    } 
}

