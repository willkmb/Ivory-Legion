using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;


public class ControllerCursor : MonoBehaviour
{
    public float Speed;
    private Vector2 Position;
    private bool notConnected = true;
    public GameObject cursor;
    private PointerEventData pointerData;
    public GraphicRaycaster UI;
    private GameObject button;
    private GameObject lastButton;

    void Awake()
    {
        Position = Input.mousePosition;
        CheckControllerConnected();
    }
            
    void Update()
    {
        CheckControllerConnected();
        float Xmove = Input.GetAxis("Horizontal");
        float Ymove = Input.GetAxis("Vertical");

        Vector2 moving = new Vector2(Xmove, Ymove) * Speed * Time.deltaTime;
        if(Position != null ) Position += moving;

        Position.x = Mathf.Clamp(Position.x, 0, Screen.width);
        Position.y = Mathf.Clamp(Position.y, 0, Screen.height);

        this.transform.position = Position;

        List<RaycastResult> hit = new List<RaycastResult>();
        UI.Raycast(new PointerEventData(EventSystem.current) { position = transform.position }, hit);
        if (hit.Count > 0) { button = hit[0].gameObject;}
        if(button != null) button = hit.Count > 0 ? hit[0].gameObject : null;

        if(button != lastButton)
        {
            if (button != null) hoverState(0);
            if (lastButton != null) hoverState(1);
            lastButton = button;
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (button != null) { if (button.GetComponentInParent<Button>() != null) { button.GetComponentInParent<Button>().onClick.Invoke(); } }
        }

        if (Input.GetKey(KeyCode.Joystick1Button0)) {this.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);}
        if (Input.GetKeyUp(KeyCode.Joystick1Button0)) { this.transform.localScale = Vector3.one; }
    }

    void CheckControllerConnected()
    {
        string[] controllers = Input.GetJoystickNames();
        for (int i = 0; i < controllers.Length; i++)
        {
            if (string.IsNullOrEmpty(controllers[i]))
            {
                notConnected = true;
            }
            else
            {
                notConnected = false; break;
            }
        }

        Cursor.visible = !notConnected ? false : true;
    }

    public void CursorState(bool state)
    {
        cursor.GetComponent<Image>().enabled = state;
        Position = Input.mousePosition;
    }

    void hoverState(int state)
    {
        if(state == 0)
        {
            if (button.transform.CompareTag("Interactable"))
            {
                Transform buttonScale = button.transform;
                button.transform.localScale = new Vector3(buttonScale.localScale.x + 0.1f, buttonScale.localScale.y + 0.1f, buttonScale.localScale.z + 0.1f);
            }
            else
            {
                if (button.transform.parent.CompareTag("Interactable"))
                {
                    Transform buttonScale = button.transform.parent;
                    button.transform.parent.localScale = new Vector3(buttonScale.localScale.x + 0.1f, buttonScale.localScale.y + 0.1f, buttonScale.localScale.z + 0.1f);
                }
            }
        }
        else if( state == 1)
        {
            if (lastButton.transform.CompareTag("Interactable"))
            {
                lastButton.transform.localScale = Vector3.one;
            }
            else
            {
                if (lastButton.transform.parent.CompareTag("Interactable"))
                {
                    lastButton.transform.parent.localScale = Vector3.one;
                }
            }
        }
    }
}
