using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Player;
using InputManager;

public class PauseScript : MonoBehaviour
{
    public GameObject pause;
    private bool isPaused = false;

    [Header("Anim Elements")]
    [SerializeField] private GameObject tint1;
    [SerializeField] private GameObject pattern;
    [SerializeField] private GameObject tint2;
    [SerializeField] private GameObject tint3;
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject hue;
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    [SerializeField] private GameObject button3;
    [SerializeField] private GameObject button1Text;
    [SerializeField] private GameObject button2Text;
    [SerializeField] private GameObject button3Text;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                resume();
            }
            else
            {
                isPaused = true;
                pause.SetActive(true);
                Debug.Log("pause");
                PlayerManager manager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
                NpcTalkTrigger tt = GameObject.FindWithTag("Player").GetComponent<NpcTalkTrigger>();
                tt.enabled = false;
                manager.movementAllowed = false;
                manager.interactionAllowed = false;
                manager.moveAction.Disable();
                manager.enabled = false;

                ControllerCursor cursor = GameObject.Find("ContCursor").GetComponent<ControllerCursor>();
                cursor.CursorState(true);

                tint1.GetComponent<Animation>().Play("TintAnim");
                pattern.GetComponent<Animation>().Play("TintAnim");
                tint2.GetComponent<Animation>().Play("TintAnim");
                tint3.GetComponent<Animation>().Play("TintAnim");
                logo.GetComponent<Animation>().Play("LogoAnimPause");
                icon.GetComponent<Animation>().Play("LogoAnimPause");
                hue.GetComponent<Animation>().Play("HueAnim");
                button1.GetComponent<Animation>().Play("ResumeButtonInAnim");
                button2.GetComponent<Animation>().Play("BottomButtonsInAnim");
                button3.GetComponent<Animation>().Play("BottomButtonsInAnim");
                button1Text.GetComponent<Animation>().Play("ResumeTextAnim");
                button2Text.GetComponent<Animation>().Play("BottomButtonText");
                button3Text.GetComponent<Animation>().Play("BottomButtonText");
            }
        }
    }

    public void resume()
    {
        isPaused = false;
        PlayerManager manager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        NpcTalkTrigger tt = GameObject.FindWithTag("Player").GetComponent<NpcTalkTrigger>();
        tt.enabled = true;
        manager.enabled = true;
        manager.movementAllowed = true;
        manager.interactionAllowed = true;
        manager.moveAction.Enable();

        ControllerCursor cursor = GameObject.Find("ContCursor").GetComponent<ControllerCursor>();
        cursor.CursorState(false);

        tint1.GetComponent<Animation>().Play("TintAnimOut");
        pattern.GetComponent<Animation>().Play("TintAnimOut");
        tint2.GetComponent<Animation>().Play("TintAnimOut");
        tint3.GetComponent<Animation>().Play("TintAnimOut");
        logo.GetComponent<Animation>().Play("LogoAnimPauseOut");
        icon.GetComponent<Animation>().Play("LogoAnimPauseOut");
        hue.GetComponent<Animation>().Play("HueAnimOut");
        button1.GetComponent<Animation>().Play("ResumeButtonOutAnim");
        button2.GetComponent<Animation>().Play("BottomButtonsOutAnim");
        button3.GetComponent<Animation>().Play("BottomButtonsOutAnim");
        button1Text.GetComponent<Animation>().Play("ResumeTextAnimOut");
        button2Text.GetComponent<Animation>().Play("BottomButtonTextOut");
        button3Text.GetComponent<Animation>().Play("BottomButtonTextOut");
        Invoke("Hide", 1f);
    }

    void Hide()
    {
        pause.SetActive(false);
    }
}
