using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Dialogue;

public class JournalTreeScript : MonoBehaviour
{
    [Header("Entry List")] 
    [SerializeField] List<entry> Entries = new List<entry>();
    [System.Serializable]
    public class entry
    {
        [Header("Required ID States")]
        public int entryID;
        [Tooltip("Leave at zero unless this entry needs a parent")]
        public int parentID;
        [Header("Optional (Only click if has more than one child!)")]
        public bool hasChildren;
    }

    [Header("Prefabs")] 
    [SerializeField] GameObject entryPrefab;
    [SerializeField] GameObject holder;
    [SerializeField] GameObject line;
    public float offsetFromParent = 10f;

    [HideInInspector] public bool isEntry = false;
    [HideInInspector] public int entryID;
    [HideInInspector] public int parentID;
    [HideInInspector] public bool hasChildren;

    private List<GameObject> EntryObjs = new List<GameObject>();
    private List<GameObject> children = new List<GameObject>();

    private bool HeightToggle = false;

    private void Awake()
    {
        if (isEntry) return;
        for (int i = 0; i < Entries.Count; i++)
        {
            GameObject instanceEntry = Instantiate(entryPrefab, holder.transform);
            instanceEntry.name = "Entry" + (i + 1);
            JournalTreeScript entryInstancedScript = instanceEntry.AddComponent<JournalTreeScript>();
            entryInstancedScript.isEntry = true;
            entryInstancedScript.entryID = Entries[i].entryID;
            entryInstancedScript.parentID = Entries[i].parentID;
            entryInstancedScript.hasChildren = Entries[i].hasChildren;

            EntryObjs.Add(instanceEntry);
        }

        for (int i = 0; i < EntryObjs.Count; i++)
        {
            if (EntryObjs[i].GetComponent<JournalTreeScript>().hasChildren)
            {
                getChildrenOfParent(EntryObjs[i].GetComponent<JournalTreeScript>().entryID);
                if (children.Count > 0)
                {
                    foreach (var child in children)
                    {
                        Debug.Log(child.name + " ID = " + child.GetComponent<JournalTreeScript>().entryID + " / " + EntryObjs[i].name);
                        HeightToggle = !HeightToggle;
                        if(HeightToggle) child.transform.Find("icon").transform.localPosition = new Vector3(child.transform.Find("icon").transform.localPosition.x, 
                                                                                EntryObjs[i].transform.Find("icon").transform.localPosition.y + offsetFromParent,
                                                                                child.transform.Find("icon").transform.localPosition.z);
                        else child.transform.Find("icon").transform.localPosition = new Vector3(child.transform.Find("icon").transform.localPosition.x - holder.GetComponent<HorizontalLayoutGroup>().spacing,
                                                                                EntryObjs[i].transform.Find("icon").transform.localPosition.y - offsetFromParent,
                                                                                child.transform.Find("icon").transform.localPosition.z);
                        DrawLine(EntryObjs[i].transform.Find("icon").gameObject, child.transform.Find("icon"). game);
                    }
                }
            }
        }
    }

    void getChildrenOfParent(int pID)
    {
        children.Clear();
        for (int i = 1; i < EntryObjs.Count; i++)
        {
            if (EntryObjs[i].GetComponent<JournalTreeScript>().parentID == pID)
            {
                children.Add(EntryObjs[i]);
            }
        }
    }

    void DrawLine(GameObject parent, GameObject child)
    {
        GameObject curLine = Instantiate(line, holder.transform);
        LineRenderer lineRend = curLine.GetComponent<LineRenderer>();
        lineRend.positionCount = 2;
        lineRend.SetPosition(0, parent.transform.position);
        lineRend.SetPosition(1, child.transform.position);
    }

    [ContextMenu("Print ID values")]
    private void printValue()
    {
        Debug.Log("Entry: " + entryID + " Parent: " + parentID + " Children: " + hasChildren);
    }
}
