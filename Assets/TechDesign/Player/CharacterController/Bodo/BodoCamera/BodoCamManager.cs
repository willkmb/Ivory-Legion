using UnityEngine;
using Unity.Cinemachine; 
using System.Collections.Generic;

public class BodoCamManager : MonoBehaviour
{
    private static List<CinemachineCamera> cameras = new List<CinemachineCamera>();

    public static CinemachineCamera ActiveCamera = null;

    public static bool IsActiveCamera(CinemachineCamera camera)
    {
        return camera == ActiveCamera;
    }

    public static void switchcamera(CinemachineCamera newCamera)
    {
        newCamera.Priority = 10;
        ActiveCamera = newCamera;
        
        foreach (CinemachineCamera cam in cameras)
            if (cam != newCamera)
            {
                cam.Priority = 0;
            }
    }

    public static void Register(CinemachineCamera camera)
    {
        cameras.Add(camera);
    }

    public static void Unregiester(CinemachineCamera camera)
    {
        cameras.Remove(camera);
    }
}

