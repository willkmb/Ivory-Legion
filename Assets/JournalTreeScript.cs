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
    [SerializeField] GameObject childHolder;
    [SerializeField] GameObject lineObj;

    [Header("Spacing")]
    public float offsetFromParent = 10f;

    [HideInInspector] public bool isEntry = false;
    [HideInInspector] public int entryID;
    [HideInInspector] public int parentID;
    [HideInInspector] public bool hasChildren;

    private List<GameObject> EntryObjs = new List<GameObject>();
    private List<GameObject> children = new List<GameObject>();

    private bool HeightToggle = false;
    private bool HeightToggleLine = false;

    private void Awake()
    {
        if (isEntry) return;
        for (int i = 0; i < Entries.Count; i++)
        {
            GameObject instanceEntry = Instantiate(entryPrefab, holder.transform);
            if (!Entries[i].hasChildren) Instantiate(lineObj, instanceEntry.transform.Find("icon"));
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
                GameObject parentObj = EntryObjs[i];
                List<GameObject> lines = new List<GameObject>();
                GameObject line1 = Instantiate(lineObj, parentObj.transform.Find("icon")); GameObject line2 = Instantiate(lineObj, parentObj.transform.Find("icon"));
                lines.Add(line1); lines.Add(line2);

                foreach (var line in lines)
                {
                    HeightToggleLine = !HeightToggleLine;
                    Transform lineRot = line.transform;
                    if(HeightToggleLine) lineRot.Rotate(0f, 0f, 30f);
                    else lineRot.Rotate(0f, 0f, -30f);
                }
                HeightToggleLine = false;
                getChildrenOfParent(EntryObjs[i].GetComponent<JournalTreeScript>().entryID);

                if (children.Count > 0)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(holder.GetComponent<RectTransform>());
                    for (int c=0 ; c<children.Count; c++)
                    {
                        Debug.Log(children[c].name + " ID = " + children[c].GetComponent<JournalTreeScript>().entryID + " / " + EntryObjs[i].name);
                        HeightToggle = !HeightToggle;
                        Transform childIcon = children[c].transform.Find("icon").transform; Transform parentIcon = EntryObjs[i].transform.Find("icon").transform;
                        if (HeightToggle)
                        {
                            childIcon.localPosition = new Vector3(childIcon.localPosition.x, parentIcon.localPosition.y + offsetFromParent, childIcon.localPosition.z);
                            childIcon.transform.Find("LinePrefab(Clone)").Rotate(0f, 0f, -30f);
                        }
                        else
                        {
                            children[c].AddComponent<LayoutElement>(); children[c].GetComponent<LayoutElement>().ignoreLayout = true;
                            children[c].transform.position = new Vector3(children[0].transform.position.x, children[0].transform.position.y, children[0].transform.position.z);
                            childIcon.transform.localPosition = new Vector3(childIcon.transform.localPosition.x, parentIcon.transform.localPosition.y - offsetFromParent, 0f);
                            childIcon.transform.Find("LinePrefab(Clone)").Rotate(0f, 0f, 30f);
                        }
                    }
                    HeightToggle = false;
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

    [ContextMenu("Print ID values")]
    private void printValue()
    {
        Debug.Log("Entry: " + entryID + " Parent: " + parentID + " Children: " + hasChildren);
    }
}
