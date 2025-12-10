using UnityEngine;
using TMPro;
using System.Collections;
public class NPCConvoTest : MonoBehaviour
{
    public GameObject char1;
    public GameObject char2;
    public GameObject bubble;
    public GameObject bubbleflip;
    public float YOffset1 = 0;
    public float YOffset2 = 0;
    void Update()
    {
        updatePos();
        if (Input.GetKeyDown(KeyCode.O))
        {
            Invoke("nextTalk1", 2f);
            bubbleflip.SetActive(false);
            bubble.GetComponent<Animation>().Play();
        }
    }

    void updatePos()
    {
        Vector3 MarkerLoc1 = new Vector3(char1.transform.position.x, char1.transform.position.y + YOffset1, char1.transform.position.z);
        Vector3 pos = Camera.main.WorldToScreenPoint(MarkerLoc1); bubble.transform.position = pos;

        Vector3 MarkerLoc2 = new Vector3(char2.transform.position.x, char2.transform.position.y + YOffset2, char2.transform.position.z);
        Vector3 pos2 = Camera.main.WorldToScreenPoint(MarkerLoc2); bubbleflip.transform.position = pos2;
    }

    void nextTalk1()
    {
        Invoke("nextTalk2", 2f);
        bubbleflip.SetActive(true);
        bubbleflip.GetComponent<Animation>().Play();
        bubble.SetActive(false);
        StartCoroutine(buttonWiggle(bubbleflip.transform, 10, 1));
    }

    void nextTalk2()
    {
        Invoke("nextTalk3", 2f);
        bubble.GetComponentInChildren<TextMeshProUGUI>().text = "Angrier";
        bubbleflip.SetActive(false);
        bubble.SetActive(true);
        bubble.GetComponent<Animation>().Play();
        StartCoroutine(buttonWiggle(bubble.transform, 11, 2));
    }

    void nextTalk3()
    {
        bubbleflip.GetComponentInChildren<TextMeshProUGUI>().text = "EVEN ANGRIER!";
        bubbleflip.SetActive(true);
        bubbleflip.GetComponent<Animation>().Play();
        bubble.SetActive(false);
        StartCoroutine(buttonWiggle(bubbleflip.transform, 12, 3));
    }

    IEnumerator buttonWiggle(Transform button, float frequency, float intensity)
    {
        float time = 0;
        float angle = 1f;
        float flip = 1f / frequency;

        while (true)
        {
            time += Time.deltaTime;
            if (time >= flip)
            {
                angle = -angle;
                time = 0f;
            }

            float setAngle = angle * intensity;
            button.localRotation = Quaternion.Euler(0, 0, setAngle);
            yield return null;
        }

    }
}
