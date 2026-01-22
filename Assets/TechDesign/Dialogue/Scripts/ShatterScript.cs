using UnityEngine;

public class ShatterScript : MonoBehaviour
{
    public float force = 5f;
    void Start()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S)) shatter();
    }

    void shatter()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 direction = transform.up;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        Vector2 direction2 = new Vector2(Random.Range(-1, 1), 1);
        rb.AddForce(direction2.normalized * force, ForceMode2D.Impulse);

    }
}
