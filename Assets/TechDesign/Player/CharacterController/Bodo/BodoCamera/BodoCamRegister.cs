using UnityEngine;
using Unity.Cinemachine;
public class BodoCamRegister : MonoBehaviour
{
    private void OnEnable()
    {
     BodoCamManager.Register(GetComponent<CinemachineCamera>());   
    }
    private void OnDisable()
    {
        BodoCamManager.Unregiester(GetComponent<CinemachineCamera>());   
    }
}
