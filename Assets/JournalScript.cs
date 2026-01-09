using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using TMPro.EditorUtilities;

public class JournalScript : MonoBehaviour
{
    public enum entryPlacements { First, Next };

    [Header("Prefabs")]
    [SerializeField] GameObject entry;
    [SerializeField] GameObject holder;
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject cursorBack;

    [Header("CapValues")]
    private int cap = 9;
    private int amount = 0;
    private int totalAmount = 0;
    private bool reachedCap = false;

    [Header("Lists")]
    private List<GameObject> entries = new List<GameObject>();
    private List<GameObject> pages = new List<GameObject>();

    [Header("Pages")]
    private int index = 0;
    private GameObject curPage;

    [Header("Numerals")]
    [SerializeField] private int Toggle = 0;
    private int num;
    private string numRoman;

    [Header("fonts")]
    [SerializeField] TMP_FontAsset romanFont;
    [SerializeField] TMP_FontAsset phonFont;
    void Update()
    {
        if (!reachedCap) { if (Input.GetKeyDown(KeyCode.Alpha1)) { InsertEntry(entryPlacements.Next, "Quest..."); } } //Test example of the function getting called on a key press
        if (amount == cap && totalAmount < 99) { reachedCap = true; cursor.SetActive(true); } //show cursor if page full
        if (index > 0) cursorBack.SetActive(true);
        else cursorBack.SetActive(false); //show back cursor if theres a previous page
        if (Input.GetKeyDown(KeyCode.Alpha0)) ClearTemp(); //Temporary code for clearing the log on key press
        if (Input.GetKeyDown(KeyCode.Alpha9)) Toggle = 1 - Toggle;
    }

    public void InsertEntry(entryPlacements placement, string content)
    {
        if (totalAmount >= 99) return; //cap at 99 total entries
        if (curPage == null) firstPage();
        GameObject current = Instantiate(entry, curPage.transform);
        if (Toggle == 0) current.GetComponent<TextMeshProUGUI>().font = romanFont;
        else current.GetComponent<TextMeshProUGUI>().font = phonFont;

        switch (placement)
            {
                case entryPlacements.First: //put entry at the first place in the list
                    Debug.Log("First");
                    entries.Insert(0, current);
                    break;
                case entryPlacements.Next: //put entry in the list consecutively
                    Debug.Log("Next");
                    entries.Add(current);
                    break;
            }

        num = (entries.IndexOf(current) + 1); //get the number value of each entry
        toRoman();
        if (numRoman != null) current.GetComponent<TextMeshProUGUI>().text = numRoman + ". " + content; //add roman numeral as a prefix
        amount++;
        totalAmount++;
    }

    void toRoman()
    {
        switch (Toggle)
        {
            case 0:
                string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
                string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
                int one = num % 10; //find ones value using modulo (value / 10, truncated and gives the remainder)
                int ten = num / 10; //find tens values by / 10
                numRoman = (tens[ten] + ones[one]); //combine the numbers
                break;

            case 1:
                string[] onesP = { "", @"\", "||", "|||", @"\|||", "|||||", "||||||", @"\||||||", "||||||||", "||||||||" };
                string[] tensP = { "", "𐤗", "𐤘", "𐤗𐤘", "𐤘𐤘", "𐤗𐤘𐤘", "𐤘𐤘𐤘", "𐤗𐤘𐤘𐤘", "𐤘𐤘𐤘𐤘", "𐤗𐤘𐤘𐤘𐤘" };
                int oneP = num % 10;
                int tenP = num / 10;
                numRoman = (onesP[oneP] + tensP[tenP]);
                break;
        }
    }

    void firstPage()
    {
        curPage = transform.Find("Tablet/ListHolderPg").gameObject;
        pages.Add(curPage); //add current page to list
        index = 0; //set the index of the page
    }

    public void nextPage()
    {
        if (index < pages.Count - 1) //if not on last page
        {
            pages[index].SetActive(false); //hide current page
            index++; //advance pages
            curPage = pages[index]; //set new current
            curPage.SetActive(true);
            amount = curPage.transform.childCount; //amount of the page is set to current amount of entries on page
            if (amount == cap) { reachedCap = true; cursor.SetActive(true); }
            else { reachedCap = false; cursor.SetActive(false); } //show/hide cursor dependant on state
        }
        else
        {
            pages[index].SetActive(false);
            curPage = Instantiate(holder, transform.Find("Tablet"));
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
        if (index > 0)
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
        for (int i = 0; i < entries.Count; i++) Destroy(entries[i]); //clears cache of entries
        entries.Clear();
        reachedCap = false;
        amount = 0;

        pages[0].SetActive(true);
        pages.RemoveAt(0);
        for (int i = 0; i < pages.Count; i++) Destroy(pages[i]); //clears cache of pages except original
        pages.Clear();
        index = 0;
        cursor.SetActive(false);
    }
}
