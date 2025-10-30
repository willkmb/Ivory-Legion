using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Camera_Change : MonoBehaviour
{
    public Transform objectToRotate;
    //public Transform requiredTrigger; // drag the correct trigger object here in Inspector
    public Vector3 targetLocalEulerAngles = new Vector3(0, 90, 0);
    public float rotationDuration = 1f; //time it takes to fully rotate

    private bool hasRotated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasRotated && other.CompareTag("Player"))
        {
            StartCoroutine(RotateToTarget());
            hasRotated = true;
        }
    }

    private IEnumerator RotateToTarget()  //begin rotate
    {
        if (objectToRotate == null)
            yield break;

        Quaternion startRotation = objectToRotate.localRotation;
        Quaternion targetRotation = Quaternion.Euler(targetLocalEulerAngles);
        float elapsed = 0f;

        while (elapsed < rotationDuration) //duration of rotate
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            objectToRotate.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        objectToRotate.localRotation = targetRotation;
        hasRotated = false;
    }
}