using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI;
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
    [SerializeField] float scale = 2.5f;

    [Header("Spacing")]
    [SerializeField] float offsetFromParent = 10f;

    [Header("colours")]
    [SerializeField] Color inactiveCol;
    [SerializeField] Color inactiveColSelected;
    [SerializeField] Color activeCol;
    [SerializeField] Color activeColSelected;

    [HideInInspector] public bool isEntry = false;
    [HideInInspector] public int entryID;
    [HideInInspector] public int parentID;
    [HideInInspector] public bool hasChildren;

    private List<GameObject> EntryObjs = new List<GameObject>();
    private List<GameObject> children = new List<GameObject>();
    private List<Transform> flatEntries = new List<Transform>();

    private bool HeightToggle = false;
    private bool HeightToggleLine = false;

    private int index;
    private Transform currentIndex;
    private int questindex = 0;
    private string numRoman;

    private void Awake()
    {
        if (isEntry) return; //don't run if its an instance
        for (int i = 0; i < Entries.Count; i++)
        {
            GameObject instanceEntry = Instantiate(entryPrefab, holder.transform);
            if (!Entries[i].hasChildren) Instantiate(lineObj, instanceEntry.transform.Find("icon"));
            instanceEntry.name = "Entry" + (i + 1);
            JournalTreeScript entryInstancedScript = instanceEntry.AddComponent<JournalTreeScript>(); //create the entry, give it a line obj, give it the script + name it

            entryInstancedScript.isEntry = true;
            entryInstancedScript.entryID = Entries[i].entryID;
            entryInstancedScript.parentID = Entries[i].parentID;
            entryInstancedScript.hasChildren = Entries[i].hasChildren; //assign all of the object data to each entry based on class values from inspector

            EntryObjs.Add(instanceEntry); //add the object ver of entry to its own list
            Transform iconVis = instanceEntry.transform.Find("icon/iconvis");
            flatEntries.Add(iconVis);
        }

        for (int i = 0; i < EntryObjs.Count; i++)
        {

            if (EntryObjs[i].GetComponent<JournalTreeScript>().hasChildren) //find the parent objects
            {
                GameObject parentObj = EntryObjs[i];
                List<GameObject> lines = new List<GameObject>();
                GameObject line1 = Instantiate(lineObj, parentObj.transform.Find("icon")); GameObject line2 = Instantiate(lineObj, parentObj.transform.Find("icon")); //give parent objects two lines
                lines.Add(line1); lines.Add(line2);

                foreach (var line in lines) // for each line, offset their rotation to face each of the childen objects
                {
                    HeightToggleLine = !HeightToggleLine;
                    Transform lineRot = line.transform;
                    if (HeightToggleLine) lineRot.Rotate(0f, 0f, 30f);
                    else lineRot.Rotate(0f, 0f, -30f);
                }
                HeightToggleLine = false;

                getChildrenOfParent(EntryObjs[i].GetComponent<JournalTreeScript>().entryID); //get a list of children based on parents ID

                if (children.Count > 0)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(holder.GetComponent<RectTransform>()); //force the horizontal layout group to build itself before creating children
                    for (int c = 0; c < children.Count; c++)
                    {
                        HeightToggle = !HeightToggle;
                        Transform childIcon = children[c].transform.Find("icon").transform; Transform parentIcon = EntryObjs[i].transform.Find("icon").transform;

                        if (HeightToggle) //position child one to be upper
                        {
                            childIcon.localPosition = new Vector3(childIcon.localPosition.x, parentIcon.localPosition.y + offsetFromParent, childIcon.localPosition.z);
                            childIcon.transform.Find("LinePrefab(Clone)").Rotate(0f, 0f, -30f); // rotate line downwards to meet the next entry
                        }
                        else // position child 2 to be lower
                        {
                            children[c].AddComponent<LayoutElement>(); children[c].GetComponent<LayoutElement>().ignoreLayout = true; //force child to ignore layout spacing

                            children[c].transform.position = new Vector3(children[0].transform.position.x, children[0].transform.position.y, children[0].transform.position.z); //set its position to the first child
                            childIcon.transform.localPosition = new Vector3(childIcon.transform.localPosition.x, parentIcon.transform.localPosition.y - offsetFromParent, 0f); //apply the offsets to the icon
                            childIcon.transform.Find("LinePrefab(Clone)").Rotate(0f, 0f, 30f);
                        }
                    }
                    HeightToggle = false; // reset toggle to true is always upper
                }
            }
        }
    }

    private void Start()
    {
        if (isEntry) return;
        EntryObjs[EntryObjs.Count - 1].transform.Find("icon/LinePrefab(Clone)").gameObject.SetActive(false); //remove last line from object in the list

        if (!Mathf.Approximately(EntryObjs[EntryObjs.Count - 1].transform.position.y, 49.997f)) //checks position of entry to detect if child, if so removes line from it and the previous entry (upper child)
        {
            EntryObjs[EntryObjs.Count - 2].transform.Find("icon/LinePrefab(Clone)").gameObject.SetActive(false);
        }

        index = 1; currentIndex = flatEntries[index]; currentIndex.localScale *= scale; changeCol(1, currentIndex.gameObject);
    }

    private void Update()
    {
        if (isEntry) return;
        float wheel = Input.mouseScrollDelta.y;
        if(wheel > 0f) { ScaleIcons(0); }
        else if(wheel < 0f) { ScaleIcons(1); }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InsertEntry("Quest...");
        }
    }

    void InsertEntry(string Content)
    {
        if (questindex > flatEntries.Count - 1) return;
        GameObject entry = flatEntries[questindex].gameObject;
        questindex++;
        entry.GetComponent<Image>().color = Color.white; changeCol(0.5f, entry.gameObject);
        entry.transform.parent.Find("text").GetComponent<TextMeshProUGUI>().text = Content;
    }
    void ScaleIcons(int state)
    {
        if(currentIndex != null) Debug.Log(currentIndex.gameObject.name);
        changeCol(.5f, currentIndex.gameObject);
        if (currentIndex.localScale.x > 1f) currentIndex.localScale /= scale;
        switch (state)
        {
            case 0: index++; break;
            case 1: index--; break;
        }
        index = Mathf.Clamp(index, 0, flatEntries.Count - 1);
        currentIndex = flatEntries[index];
        currentIndex.localScale *= scale;
        changeCol(1, currentIndex.gameObject);
        currentIndex.GetComponentInChildren<Animation>().Play();

    }

    void changeCol(float amount, GameObject obj)
    {
        Image img = obj.GetComponentInChildren<Image>();
        Color curCol = img.color; curCol.a = amount; img.color = curCol;
    }

    void ToRoman()
    {
        string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
        string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
        int one = index % 10;
        int ten = index / 10;
        numRoman = (tens[ten] + ones[one]);
    }

    void getChildrenOfParent(int pID)
    {
        children.Clear();
        for (int i = 1; i < EntryObjs.Count; i++)
        {
            if (EntryObjs[i].GetComponent<JournalTreeScript>().parentID == pID)
            {
                children.Add(EntryObjs[i]); //find all children with matching parent IDs for child spacing above
            }
        }
    }

    [ContextMenu("Print ID values")] // creates a button that can be clicked on the script of entries to show their inherited ID values in the console
    private void printValue()
    {
        Debug.Log("Entry: " + entryID + " Parent: " + parentID + " Children: " + hasChildren);
    }
}
