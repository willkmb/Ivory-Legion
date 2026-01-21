using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class BodoCamSwitch : MonoBehaviour
{
    public CinemachineCamera MainCam;
    public CinemachineCamera Vcam_Far;
    public CinemachineCamera Vcam_Close;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            BodoCamManager.switchcamera(MainCam);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            BodoCamManager.switchcamera(Vcam_Far);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            BodoCamManager.switchcamera(Vcam_Close);
        }
    }
}
