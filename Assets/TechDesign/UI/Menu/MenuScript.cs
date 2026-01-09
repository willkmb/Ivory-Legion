using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public float speed = 90f;            
    public float rotateAmount = 45f;     
    public TMP_Text uiText;              

    private float turned;
    private bool rotating;
    private int textIndex = 0;
    private string[] texts = { "Settings", "About", "Quit", "Play" };
    private int state = 3;
    public Animation button;
    public GameObject transition;

    private int rotateDirection = 1;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.JoystickButton4) && !rotating)
        {
            rotateDirection = 1;
            rotating = true;
            turned = 0f;

            StartCoroutine(ChangeText());
            button.Play();
            state++;
            if (state >= texts.Length) { state = 0; }
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.JoystickButton5) && !rotating)
        {
            rotateDirection = -1;
            rotating = true;
            turned = 0f;

            StartCoroutine(ChangeText());
            button.Play();
            state--;
            if (state < 0) state = texts.Length - 1;
        }

        if (rotating)
        {
            float step = speed * Time.deltaTime;
            if (turned + step > rotateAmount) { step = rotateAmount - turned; }
            transform.Rotate(0, 0, step * rotateDirection);
            turned += step;
            if (turned >= rotateAmount) { rotating = false; }
        }

        if (state == 3 && Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            transition.GetComponent<Animation>().Play();
            Invoke("changeScene", 1f);
        }
    }

    private void changeScene()
    {
        SceneManager.LoadScene(1);
    }

    public IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(0.15f);
        textIndex = state % texts.Length;
        uiText.text = texts[textIndex];
    }
}
