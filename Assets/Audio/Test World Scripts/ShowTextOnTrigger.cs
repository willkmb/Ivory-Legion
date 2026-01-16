using TMPro;
using UnityEngine;

public class ShowTextOnTrigger : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private TextMeshProUGUI  textMeshPro;

    private void OnTriggerEnter(Collider other)
    {
        textMeshPro.enabled = true;
        textMeshPro.text = text;
    }

    private void OnTriggerExit(Collider other)
    {
        textMeshPro.enabled = false;
        textMeshPro.text = "";
    }
}
