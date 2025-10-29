using UnityEngine;
using TMPro;

public class BouncingText : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float amplitude = 10f; // how high it moves
    public float frequency = 2f;  // how fast it moves

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(0, yOffset, 0);
    }
}

