using UnityEngine;
using UnityEngine.Events;

namespace InputManager
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        
        public event UnityAction EpressEvent;
        public event UnityAction IpressEvent;

        private void Awake()
        {
            instance ??= this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                EpressEvent?.Invoke();

            if (Input.GetKeyDown(KeyCode.I))
                IpressEvent?.Invoke();
        }
    }
}
