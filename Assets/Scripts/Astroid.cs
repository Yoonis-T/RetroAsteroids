using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public int size = 3;
    [SerializeField] private float moveForce = 5f;

    [Header("References")]
    public GameManager gameManager;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject explosionPrefab;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 direction = Random.insideUnitCircle.normalized;
        rb.AddForce(direction * moveForce, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-1f, 1f), ForceMode2D.Impulse);

        // Scale asteroid based on size
        float scale = 1f;
        if (size == 2) scale = 0.7f;
        if (size == 1) scale = 0.4f;

        transform.localScale = Vector3.one * scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        Destroy(collision.gameObject);

        // Explosion
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.transform.localScale = Vector3.one * 0.5f; // smaller explosion
        }

        if (gameManager != null)
            gameManager.AddScore(10);

        // Split asteroid
        if (size > 1 && asteroidPrefab != null)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject newAsteroid = Instantiate(
                    asteroidPrefab,
                    transform.position,
                    Quaternion.identity
                );

                newAsteroid.tag = "Asteroid";

                Asteroid asteroidScript = newAsteroid.GetComponent<Asteroid>();
                asteroidScript.size = size - 1;
                asteroidScript.gameManager = gameManager;
            }
        }

        Destroy(gameObject);
    }
}