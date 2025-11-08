using UnityEngine;

public class PromptScript : MonoBehaviour
{
    public GameObject Prompt;
    private GameObject thisPrompt;
    public Transform rootPrompt;
    [Range(0, 20)]
    public float YOffset = 0;
    void Awake()
    {
        populate();
    }

    void Update()
    {
        /*Renderer rend = GetComponentInChildren<Renderer>();
        bool isVisible = rend.isVisible;
        if (isVisible)
        {
            Vector3 MarkerPos = new Vector3(this.transform.position.x, this.transform.position.y + YOffset, this.transform.position.z);
            Vector3 pos = Camera.main.WorldToScreenPoint(MarkerPos);
            thisPrompt.transform.position = pos;
        }*/

        Vector3 MarkerPos = new Vector3(this.transform.position.x, this.transform.position.y + YOffset, this.transform.position.z);
        Vector3 pos = Camera.main.WorldToScreenPoint(MarkerPos);
        thisPrompt.transform.position = pos;
    }

    public void populate()
    {
        if (this.CompareTag("NPC")) thisPrompt = Instantiate(Prompt, rootPrompt);
    }
}
