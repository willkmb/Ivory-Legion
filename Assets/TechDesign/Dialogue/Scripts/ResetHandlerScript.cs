using Player;
using UnityEngine;

public class ResetHandlerScript : MonoBehaviour
{
    public GameObject handler;
    public GameObject pushObject;
    public GameObject pickup1;
    public GameObject pickup2;
    public GameObject pickup3;


    private void Start()
    {
        pushObject.GetComponent<PromptScript>().thisPrompt.SetActive(false);
        Invoke("hidePickup", 2f);
    }
    public void Reset()
    {
        handler.GetComponent<PromptScript>().thisPrompt.SetActive(true);
        handler.GetComponent<Dialogue>().enabled = true;
        pushObject.tag = "Pushable";
        pushObject.GetComponent<PromptScript>().thisPrompt.SetActive(true);
    }

    void startVase()
    {
        pickup1.GetComponent<PromptScript>().thisPrompt.SetActive(true);
        pickup2.GetComponent<PromptScript>().thisPrompt.SetActive(true);
        pickup3.GetComponent<PromptScript>().thisPrompt.SetActive(true);

        pickup1.GetComponent<PickUpPutDownScript>().enabled = true;
        pickup2.GetComponent<PickUpPutDownScript>().enabled = true;
        pickup3.GetComponent<PickUpPutDownScript>().enabled = true;
    }

    void hidePickup()
    {
        pickup1.GetComponent<PromptScript>().thisPrompt.SetActive(false);
        pickup2.GetComponent<PromptScript>().thisPrompt.SetActive(false);
        pickup3.GetComponent<PromptScript>().thisPrompt.SetActive(false);
    }

    private void Update()
    {
        PushController push = GameObject.FindWithTag("Player").GetComponent<PushController>();
        if (push.pushedOnce) { pushObject.GetComponent<PromptScript>().thisPrompt.SetActive(false); startVase(); return; }
    }
}
