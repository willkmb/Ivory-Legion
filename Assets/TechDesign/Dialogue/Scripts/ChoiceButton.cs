using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Player.Dialogue
{ 
    public class ChoiceButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        public void Selected()
        {
            _button.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        }

        public void Unselected()
        {
            _button.transform.localScale = new Vector2(1f, 1f);
        }

        public void Interacted()
        {
            Debug.Log("Clicked");
        }
    }
}

