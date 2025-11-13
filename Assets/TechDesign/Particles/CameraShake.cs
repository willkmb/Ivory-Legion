using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public float duration = 1f;

    private Vector3 originalLocalPos;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        originalLocalPos = transform.localPosition;
    }

    public void Shake()
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float normalizedTime = elapsedTime / duration;

            float strength = curve.Evaluate(normalizedTime);

            transform.localPosition = originalLocalPos + Random.insideUnitSphere * strength;

            yield return null;
        }

        float t = 0f;
        Vector3 startPos = transform.localPosition;

        while (t < 0.2f)
        {
            t += Time.deltaTime / 0.2f;
            transform.localPosition = Vector3.Lerp(startPos, originalLocalPos, t);
            yield return null;
        }

        transform.localPosition = originalLocalPos;
        shakeRoutine = null;
    }
}
