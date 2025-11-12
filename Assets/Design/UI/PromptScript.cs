using UnityEngine;
using UnityEngine.UI;

public class PromptScript : MonoBehaviour
{
    public GameObject Prompt;
    public Renderer rend;
    [HideInInspector] public GameObject thisPrompt;
    public Transform rootPrompt;
    [Range(0, 20)]
    public float YOffset = 0;
    void Awake()
    {
        populate();
    }

    void Update()
    {
        bool isVisible = rend.isVisible;
        if (isVisible)
        {
            thisPrompt.GetComponent<Image>().enabled = true;
            Vector3 MarkerPos = new Vector3(this.transform.position.x, this.transform.position.y + YOffset, this.transform.position.z);
            Vector3 pos = Camera.main.WorldToScreenPoint(MarkerPos);
            thisPrompt.transform.position = pos;
        }
        else
        {
            thisPrompt.GetComponent<Image>().enabled = false;
        }
    }

    public void populate()
    {
        if (this.CompareTag("NPC") || this.CompareTag("Pushable") || this.CompareTag("Interactable")) thisPrompt = Instantiate(Prompt, rootPrompt);
    }
}
