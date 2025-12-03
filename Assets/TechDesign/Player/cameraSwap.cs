using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cameraSwap : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> cameraTexts;
    [SerializeField] private List<Camera> cameras;

    public void swap(int camIndex)
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].gameObject.SetActive(false);
            Color textAlpha = cameraTexts[i].color;
            textAlpha.a = 0.5f;
            cameraTexts[i].color = textAlpha;
        }

        cameras[camIndex].gameObject.SetActive(true);
        Color textAlpha2 = cameraTexts[camIndex].color;
        textAlpha2.a = 1;
        cameraTexts[camIndex].color = textAlpha2;
    }
}
