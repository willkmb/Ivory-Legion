using UnityEngine;

namespace World
{
    public class IAmWorldBlocker : MonoBehaviour
    {
        private void Awake()
        {
            MeshRenderer meshRenderer = transform.gameObject.GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
        }
    }
}

