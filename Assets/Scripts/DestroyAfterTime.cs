using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time = 1f;

    void Start()
    {
        Destroy(gameObject, time);
    }
}