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
    public Animation button;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !rotating)
        {
            rotating = true;
            turned = 0f;

            StartCoroutine(ChangeText());
            button.Play();
        }

        if (rotating)
        {
            float step = speed * Time.deltaTime;
            transform.Rotate(0, step, 0);
            turned += step;

            if (turned >= rotateAmount)
                rotating = false;
        }
    }

    public IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(0.15f);
        uiText.text = texts[textIndex];
        textIndex = (textIndex + 1) % texts.Length;
    }
}
