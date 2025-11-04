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

        private bool isArrow;
        private bool isLastArrow;

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
            if (isArrow)
            {
                root.branchIndex++;
                root.branchIndex = nextBranch;
            }
            else if (isLastArrow)
            {
                root.HideDialogue();
            }
            else
            {
                root.branchIndex = nextBranch;
                root.UpdateNPCOpinion(topic);
                root.ShowNextBranch();
            }
        }
    }
}
