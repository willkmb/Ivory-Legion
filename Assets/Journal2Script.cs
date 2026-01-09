using System.Collections.Generic;
using UnityEngine;

public class Journal2Script : MonoBehaviour
{
    [SerializeField] GameObject icon;
    private List<GameObject> entries = new List<GameObject>();
    private int index = 0;
    private int mouseIndex = 0;
    public float spacing = 0f;
    public float scaling = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) createEvent("Quest...");
        float scrolling = Input.mouseScrollDelta.y;
        if (scrolling > 0f) mouseIndex++;
       // else mouseIndex--;

        Transform mouseCur = entries[mouseIndex].transform;
        Vector3 scale = mouseCur.localScale;
        scale *= scaling;
        mouseCur.localScale = scale;

        Debug.Log(mouseIndex);
    }

    void createEvent(string content)
    {
        GameObject cur = Instantiate(icon, transform.Find("ListHolder"));
        entries.Add(cur);
        if (entries.Count > 0) 
        {
            index++;
            Transform curT = entries[index - 1].transform;
            curT.position = entries[index - 2].transform.position;
            Vector3 position = curT.position;
            position.x += spacing;
            curT.position = position;
        }
    }
}
