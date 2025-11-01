using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControllerCursor : MonoBehaviour
{
    public float Speed;
    public float offset;
    private Vector2 Position;
    private bool notConnected = true;
    public GameObject cursor;
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

        Position.x = Mathf.Clamp(Position.x, 0, Screen.width - offset);
        Position.y = Mathf.Clamp(Position.y, 0, Screen.height - offset);

        this.transform.position = Position;

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Debug.Log("KeyPress");
            PointerEventData click = new PointerEventData(EventSystem.current);
            click.position = Position;
            List<RaycastResult> hitResult = new List<RaycastResult>();
            EventSystem.current.RaycastAll(click, hitResult);
            if (hitResult.Count > 0) ExecuteEvents.Execute(hitResult[0].gameObject, click, ExecuteEvents.pointerClickHandler);
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

        Cursor.visible = notConnected ? false : true;
    }

    public void CursorState(bool state)
    {
        cursor.GetComponent<Image>().enabled = state;
        //Position = Input.mousePosition;
    }
}
