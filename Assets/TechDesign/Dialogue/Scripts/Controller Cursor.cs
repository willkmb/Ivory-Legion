using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;


public class ControllerCursor : MonoBehaviour
{
    public float Speed;
    private Vector2 Position;
    private bool notConnected = true;
    public GameObject cursor;
    private PointerEventData pointerData;
    public GraphicRaycaster UI;
    private GameObject button;

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
        if (hit.Count > 0) { Debug.Log(hit[0]); button = hit[0].gameObject;}

        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (button != null) { button.GetComponentInParent<Button>().onClick.Invoke(); }
        }
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
}
