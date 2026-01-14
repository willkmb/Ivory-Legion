using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Journal2Script : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject holder;
    [SerializeField] GameObject icon;
    [SerializeField] private float scale = 2.5f;

    [Header("Colours")]
    [SerializeField] private float incol = .1f;
    [SerializeField] private float outCol = 1f;

    [Header("Lists")]
    private List<GameObject> entries = new List<GameObject>();
    private int cap = 15;

    [Header("index")]
    private int index = 0; //scroll index
    private GameObject currentIndex;
    private int questIndex = 0; //insert quest entry index
    private string numRoman;
    
    private void Awake()
    {
        for(int i = 0; i < cap; i++) { GameObject cur = Instantiate(icon, holder.transform); entries.Add(cur); }
        for (int i = 0; i < cap; i += 2) 
        { 
            Transform t = entries[i].transform.Find("icon/text"); t.localPosition = new Vector3(t.localPosition.x, 85f, t.localPosition.z);
            Transform p = entries[i].transform.Find("popUp"); p.localPosition = new Vector3(p.localPosition.x, 85f, p.localPosition.z);
            p.localRotation = Quaternion.Euler(0f, 0f, 180f);
        }
        currentIndex = entries[cap/2]; index = cap/2; currentIndex.transform.localScale *= scale;
        changeCol(outCol, currentIndex);
    }

    private void Update()
    {
        float wheel = Input.mouseScrollDelta.y;

        if (wheel > 0f) { scaleIcon(0); }
        else if(wheel < 0f) { scaleIcon(1); }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InsertEntry("Quest...");
        }


    }

    void InsertEntry(string content)
    {
        if (questIndex > cap-1 )
            return; 

        GameObject entry = entries[questIndex];
        questIndex++;
        Button button = entry.transform.Find("icon").gameObject.AddComponent<Button>();
        button.onClick.AddListener(() => spawnPopup(entry));
        entry.GetComponentInChildren<Image>().color = Color.white;
        if (index != questIndex - 1) changeCol(incol, entry);
        else changeCol(outCol, entry); entry.transform.Find("icon").GetComponent<Button>().enabled = false;
        ToRoman();
        entry.transform.Find("icon/text").GetComponent<TextMeshProUGUI>().text = numRoman + ". " + content;
    }

    void ToRoman()
    {
        string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
        string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
        int one = questIndex % 10;
        int ten = questIndex / 10;
        numRoman = (tens[ten] + ones[one]);
    }

    void scaleIcon(int state)
    {
        if(currentIndex.GetComponentInChildren<Button>() != null) currentIndex.GetComponentInChildren<Button>().enabled = false;
        changeCol(incol, currentIndex);
        currentIndex.transform.localScale /= scale;
        switch (state)
        {
            case 0: index++; break;
            case 1: index--; break;
        }
        index = Mathf.Clamp(index, 0, cap - 1); 
        currentIndex = entries[index]; 
        currentIndex.transform.localScale *= scale;
        changeCol(outCol, currentIndex);
        if (currentIndex.GetComponentInChildren<Button>() != null) currentIndex.GetComponentInChildren<Button>().enabled = true;
        currentIndex.GetComponentInChildren<Animation>().Play();

        GameObject[] popups;
        popups = GameObject.FindGameObjectsWithTag("popUp");
        foreach (GameObject popup in popups) { popup.SetActive(false); }
    }

    void changeCol(float amount, GameObject obj)
    {
        Image img = obj.GetComponentInChildren<Image>();
        Color curCol = img.color; curCol.a = amount; img.color = curCol;
    }

    void spawnPopup(GameObject entry)
    {
        entry.transform.Find("popUp").gameObject.SetActive(true);
    }
}
