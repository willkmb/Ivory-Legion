using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
public enum entryPlacements { First, Next};

public class JournalScript : MonoBehaviour
{
    [SerializeField] GameObject entry;
    [SerializeField] GameObject holder;
    [SerializeField] int cap = 9;
    private int amount = 0;
    private int totalAmount = 0;
    private bool reachedCap = false;
    private bool newpage = false;
    private List<GameObject> entries = new List<GameObject>();
    private List<GameObject> pages = new List<GameObject>();

    private int num;
    private string numRoman;

    [SerializeField] GameObject cursor;
    [SerializeField] GameObject cursorBack;
    private int index = 0;
    private GameObject curPage;
    void Update()
    {
        if(!reachedCap) { if(Input.GetKeyDown(KeyCode.Alpha1)) { InsertEntry(entryPlacements.Next, "Quest...");}}
        if (amount == cap && totalAmount < 99) { reachedCap = true; cursor.SetActive(true); }
        if(index > 0) cursorBack.SetActive(true);
        else cursorBack.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Alpha0)) ClearTemp();
    }

    public void InsertEntry(entryPlacements placement, string content)
    {
        if (totalAmount >= 99) return;
        if (curPage == null) firstPage();
        GameObject current = Instantiate(entry, curPage.transform);

            switch (placement)
            {
                case entryPlacements.First:
                    Debug.Log("First");
                    entries.Insert(0, current);
                    break;
                case entryPlacements.Next:
                    Debug.Log("Next");
                    entries.Add(current);
                    break;
            }

        num = (entries.IndexOf(current) + 1);
        toRoman();
        if (numRoman != null) current.GetComponent<TextMeshProUGUI>().text = numRoman + ". " + content;
        amount++;
        totalAmount++;
    }

    void toRoman()
    {
        string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
        string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
        int one = num % 10;
        int ten = num / 10;
        numRoman = (tens[ten] + ones[one]);
    }

    void firstPage()
    {
        curPage = transform.Find("ListHolderPg").gameObject;
        pages.Add(curPage);
        index = 0;
    }

    public void nextPage()
    {
        if (index < pages.Count - 1)
        {
            pages[index].SetActive(false);
            index++;
            curPage = pages[index];
            curPage.SetActive(true);
            amount = curPage.transform.childCount;
            if(amount == cap) { reachedCap = true; cursor.SetActive(true); }
            else { reachedCap = false; cursor.SetActive(false); }
        }
        else
        {
            pages[index].SetActive(false);
            curPage = Instantiate(holder, this.transform);
            pages.Add(curPage);
            index++;
            curPage.SetActive(true);
            reachedCap = false;
            amount = 0;
            cursor.SetActive(false);
        }
    }

    public void prevPage()
    {
        if(index > 0)
        {
            pages[index].SetActive(false);
            index--;
            curPage = pages[index];
            curPage.SetActive(true);
            amount = cap;
        }
    }

    void ClearTemp()
    {
        for (int i = 0; i < entries.Count; i++) Destroy(entries[i]);
        entries.Clear();
        reachedCap = false;
        amount = 0;
    }
}
