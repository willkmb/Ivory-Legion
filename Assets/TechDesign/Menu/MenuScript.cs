using UnityEngine;
using TMPro;
using System.Collections;

public class MenuScript : MonoBehaviour
{
    public float speed = 90f;            
    public float rotateAmount = 45f;     
    public TMP_Text uiText;              

    private float turned;
    private bool rotating;
    private int textIndex = 0;
    private string[] texts = { "Settings", "About", "Quit", "Play" };
    private int state = 0;
    public Animation button;
    public GameObject transition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !rotating)
        {
            rotating = true;
            turned = 0f;

            StartCoroutine(ChangeText());
            button.Play();
            state++;
            if(state > 3) { state = 0; }
        }

        if (rotating)
        {
            float step = speed * Time.deltaTime;
            if (turned + step > rotateAmount)
                step = rotateAmount - turned;
            transform.Rotate(0, 0, step);
            turned += step;

            if (turned >= rotateAmount)
                rotating = false;
        }

        if (state == 0 && Input.GetKeyDown(KeyCode.P))
        {
            transition.GetComponent<Animation>().Play();
        }
    }

    public IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(0.15f);
        uiText.text = texts[textIndex];
        textIndex = (textIndex + 1) % texts.Length;
    }
}
