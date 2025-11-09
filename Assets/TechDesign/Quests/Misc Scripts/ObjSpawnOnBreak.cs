using System;
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
    public class ObjSpawnOnBreak : MonoBehaviour
    {
        [SerializeField] private GameObject spawnObj;
        public OnDestructionType onDestruction;

        private void Start()
        {
            if (onDestruction == OnDestructionType.EnableObj)
                spawnObj.SetActive(false);
        }

        private void OnDisable()
        {
            switch (onDestruction)
            {
                case OnDestructionType.Instantiate:
                    GameObject go = Instantiate(spawnObj, transform.position, Quaternion.identity);
                    break;
                case OnDestructionType.EnableObj:
                    spawnObj.gameObject.SetActive(false);
                    spawnObj.transform.position = transform.position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    } 
}

