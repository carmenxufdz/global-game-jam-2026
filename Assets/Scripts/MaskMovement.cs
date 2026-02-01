using UnityEngine;

public class MaskMovement : MonoBehaviour
{
    public float amplitude = 0.2f;
    public float frequency = 1f;   

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + Vector3.up * yOffset;
    }
}