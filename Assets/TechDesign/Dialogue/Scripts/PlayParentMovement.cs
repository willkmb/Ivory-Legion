using InputManager;
using UnityEngine;

public class PlayParentMovement : MonoBehaviour
{
    public GameObject parent1;
    public GameObject parent2;

    public Transform p1;
    public Transform p2;

    public void move()
    {
        PlayerManager.instance.inCutscene = true;
        parent1.GetComponent<Animation>().Play();
        parent2.GetComponent<Animation>().Play();
        Invoke("endmovement", parent1.GetComponent<Animation>().clip.length);
    }

    public void endmovement()
    {
        PlayerManager.instance.inCutscene = false;
        GetComponent<ResetHandlerScript>().Reset();
        parent1.GetComponent<Animation>().Stop();
        parent2.GetComponent<Animation>().Stop();
        parent1.transform.position = p1.position;
        parent2.transform.position = p2.position;
        Vector3 p1Rot = parent1.transform.localEulerAngles;
        parent1.transform.localEulerAngles = new Vector3 (p1Rot.x, 0f, p1Rot.z);
        Vector3 p2Rot = parent1.transform.localEulerAngles;
        parent2.transform.localEulerAngles = new Vector3 (p2Rot.x, 0f, p2Rot.z);
    }
}
