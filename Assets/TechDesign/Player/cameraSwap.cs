using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cameraSwap : MonoBehaviour
{
    [Header("Camera Lists")]
    [SerializeField] private List<TextMeshProUGUI> cameraTexts;
    [SerializeField] private List<Camera> cameras;
    [SerializeField] private TextMeshProUGUI orthpers;
    [SerializeField] private Slider fovSlider;
    [Header("Rotation")]
    [SerializeField] private Slider xRotSlider;
    [SerializeField] private Slider yRotSlider;
    [SerializeField] private Slider zRotSlider;
    [Header("Offset")]
    [SerializeField] private Slider XoffSlider;
    [SerializeField] private Slider yOffSlider;
    [SerializeField] private Slider zOffSlider;
    [SerializeField] private TextMeshProUGUI offText;
    private Camera current;
    private bool state = false;
    private bool fp = false;

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

        current = cameras[camIndex];

        switch (camIndex)
        {
            case 0 or 2 or 3 or 4:
                Color alpha = orthpers.color;
                alpha.a = 1;
                orthpers.color = alpha;
                fp = false;
                break;

            case 1:
                Color alpha2 = orthpers.color;
                alpha2.a = 0.5f;
                orthpers.color = alpha2;
                fp = true;
                break;
        }

        if (!current.GetComponent<FollowPlayer>())
        {
            Color alphaOff = offText.color;
            alphaOff.a = 0.5f;
            offText.color = alphaOff;
        }
        else
        {
            Color alphaOff = offText.color;
            alphaOff.a = 1f;
            offText.color = alphaOff;
        }

            initialising();
    }

    public void switchPers()
    {
        if (!fp)
        {
            state = !state;
            if (state) { orthpers.text = "Pers..."; if (current != null) { current.orthographic = true; } }
            else { orthpers.text = "Ortho..."; if (current != null) { current.orthographic = false; } }
        }
    }

    public void fov()
    {
        if (state) { current.orthographicSize = fovSlider.value; fovSlider.minValue = 1; fovSlider.maxValue = 20; }
        else { current.fieldOfView = fovSlider.value; fovSlider.minValue = 10; fovSlider.maxValue = 100; }
    }

    public void rotSliders()
    {
        float xRot = xRotSlider.value;
        float yRot = yRotSlider.value;
        float zRot = zRotSlider.value;

        current.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
    }

    public void offsetSliders()
    {
        if (!current.GetComponent<FollowPlayer>()) return;

        FollowPlayer follow = current.GetComponent<FollowPlayer>();
        follow.xAdjust = XoffSlider.value;
        follow.yAdjust = yOffSlider.value;
        follow.zAdjust = zOffSlider.value;

    }

    public void saveToClipBoard()
    {
        string viewCurrent = current.orthographic ? "Orthographic" : "Perspective";
        if (current.GetComponent<FollowPlayer>())
        {
            string Preset = "Camera Preset" + System.Environment.NewLine +
            "Camera:    " + current.name + System.Environment.NewLine +
            "View:  " + viewCurrent + System.Environment.NewLine +
            System.Environment.NewLine +
            "X Rotation:    " + current.transform.rotation.eulerAngles.x + System.Environment.NewLine +
            "Y Rotation:    " + current.transform.rotation.eulerAngles.y + System.Environment.NewLine +
            "Z Rotation:    " + current.transform.rotation.eulerAngles.z + System.Environment.NewLine +
            System.Environment.NewLine +
            "X Offset   " + current.GetComponent<FollowPlayer>().xAdjust + System.Environment.NewLine +
            "Y Offset   " + current.GetComponent<FollowPlayer>().yAdjust + System.Environment.NewLine +
            "Z Offset   " + current.GetComponent<FollowPlayer>().zAdjust + System.Environment.NewLine;

            GUIUtility.systemCopyBuffer = Preset;
            Debug.Log("Copied: " + Preset);
        }
        else
        {
            string Preset = "Camera Preset" + System.Environment.NewLine +
            "Camera:    " + current.name + System.Environment.NewLine +
            "View:  " + viewCurrent + System.Environment.NewLine +
            System.Environment.NewLine +
            "X Rotation:    " + current.transform.rotation.eulerAngles.x + System.Environment.NewLine +
            "Y Rotation:    " + current.transform.rotation.eulerAngles.y + System.Environment.NewLine +
            "Z Rotation:    " + current.transform.rotation.eulerAngles.z + System.Environment.NewLine;

            GUIUtility.systemCopyBuffer = Preset;
            Debug.Log("Copied: " + Preset);
        }
    }

    void initialising()
    {
        if (state) { fovSlider.value = current.orthographicSize; fovSlider.value = 6; }
        else { fovSlider.value = current.fieldOfView; fovSlider.value = 30; }

        xRotSlider.value = current.transform.rotation.eulerAngles.x;
        yRotSlider.value = current.transform.rotation.eulerAngles.y;
        zRotSlider.value = current.transform.rotation.eulerAngles.z;

        FollowPlayer follow = current.GetComponent<FollowPlayer>();
        XoffSlider.value = follow.xAdjust;
        yOffSlider.value = follow.yAdjust;
        zOffSlider.value = follow.zAdjust;
    }

    private void Update()
    {
        Debug.Log(current.name);

    }

    private void Start()
    {
        current = cameras[0];

        initialising();
    }
}
