using UnityEngine;

public class CreditsScript : MonoBehaviour
{
    public GameObject logo;
    public GameObject credits;
    public GameObject list1;
    public GameObject list2;
    public GameObject list3;
    public GameObject list4;
    public GameObject final;

    public float delaystart = 0f;
    public float delaysecond = 0f;
    public float delaythird = 0f;
    public float delayfourth = 0f;
    public float delayfifth = 0f;
    public float delaysixth = 0f;
    void Start()
    {
        Invoke("StartAnim", delaystart);
    }

    void StartAnim()
    {
        logo.GetComponent<Animation>().Play();
        credits.GetComponent<Animation>().Play();
        Invoke("firstList", delaysecond);
    }

    void firstList()
    {
        list1.GetComponent<Animation>().Play();
        Invoke("secondList", delaythird);
    }

    void secondList()
    {
        list2.GetComponent<Animation>().Play();
        Invoke("thirdList", delayfourth);
    }

    void thirdList()
    {
        list3.GetComponent<Animation>().Play();
        Invoke("fourthList", delayfifth);
    }

    void fourthList()
    {
        list4.GetComponent<Animation>().Play();
        Invoke("finalline", delaysixth);
    }

    void finalline()
    {
        final.GetComponent<Animation>().Play();
    }
}
