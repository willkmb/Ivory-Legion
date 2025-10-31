using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Player.Dialogue
{ 
    public class ChoiceButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        public global::Dialogue root;
        public int nextBranch;
        public string topic;

        private void Awake()
        {
            _button.onClick.AddListener(Interacted);
        }

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
            //UIInputManager.instance.dialogueInUse.ShowNextBranch();
            root.branchIndex = nextBranch;
            root.UpdateNPCOpinion(topic);
            root.ShowNextBranch();
        }
    }
}
