using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float thrustSpeed = 10f;
    [SerializeField] private float turnSpeed = 200f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private Rigidbody2D rb;
    private bool thrusting;
    private float turnDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        if (keyboard == null) return;

        thrusting =
            keyboard.wKey.isPressed ||
            keyboard.upArrowKey.isPressed;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            turnDirection = 1f;
        }
        else if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            turnDirection = -1f;
        }
        else
        {
            turnDirection = 0f;
        }

        // Shoot with SPACE or Right Mouse
        if (keyboard.spaceKey.wasPressedThisFrame ||
            mouse.rightButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            rb.AddForce(transform.up * thrustSpeed);
        }

        if (turnDirection != 0f)
        {
            rb.AddTorque(turnDirection * turnSpeed);
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Project(firePoint.up);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            FindObjectOfType<GameManager>().PlayerDied();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            FindObjectOfType<GameManager>().PlayerDied();
            Destroy(gameObject);
        }
    }
}