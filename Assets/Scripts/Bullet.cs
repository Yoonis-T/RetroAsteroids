using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float maxLifetime = 2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, maxLifetime);
    }
}