using Unity.VisualScripting;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float fadeSpeed;
    [SerializeField] float minAlpha;
    private Shader shader;
    private GameObject hit;
    bool hasHit = false;
    bool faded = false;
    private bool debugToggle;

    private void Start()
    {
        shader = Shader.Find("Shader Graphs/dithershader");
    }
    private void Update()
    {
        Debug.Log("faded" + faded);
        bool hitting = false;
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < hits.Length; i++)
        {
            Renderer rend = hits[i].GetComponent<Renderer>();
            if (rend != null && rend.sharedMaterial?.shader == shader) { hitting = true; hit = hits[i].gameObject; break; }
        }

        if (hitting && !hasHit) {minAlpha = 0.5f; faded = true;}
        else if (!hitting && hasHit) {minAlpha = 1; faded = false;}
        hasHit = hitting;

        Vector3 direction = transform.position - hit.transform.position;
        float dotProduct = Vector3.Dot(Camera.main.transform.forward, direction);
        if (dotProduct < 0 && faded) return;

        if (hit != null)
        {
            Renderer rend = hit.GetComponent<Renderer>();
            if(rend.material.GetFloat("_State") == 1f)
            {
                Color inCol = rend.material.GetColor("_MainCol");
                inCol.a = Mathf.Lerp(inCol.a, minAlpha, fadeSpeed * Time.deltaTime);
                rend.material.SetColor("_MainCol", inCol);
            }
            else
            {
                Color inCol = rend.material.GetColor("_Top_Alpha");
                inCol.a = Mathf.Lerp(inCol.a, minAlpha, fadeSpeed * Time.deltaTime);
                rend.material.SetColor("_Top_Alpha", inCol);
            }
            
        }
    }

    [ContextMenu("Toggle Debug Sphere")]
    void toggleSphere()
    {
        debugToggle = !debugToggle;
    }

    void OnDrawGizmos()
    {
        if (debugToggle) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
