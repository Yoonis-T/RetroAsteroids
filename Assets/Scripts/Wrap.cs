using UnityEngine;

public class Wrap : MonoBehaviour
{
    private Camera cam;
    private Vector2 screenBounds;

    private void Start()
    {
        cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );
    }

    private void LateUpdate()
    {
        Vector3 position = transform.position;

        if (position.x > screenBounds.x)
            position.x = -screenBounds.x;
        else if (position.x < -screenBounds.x)
            position.x = screenBounds.x;

        if (position.y > screenBounds.y)
            position.y = -screenBounds.y;
        else if (position.y < -screenBounds.y)
            position.y = screenBounds.y;

        transform.position = position;
    }
}