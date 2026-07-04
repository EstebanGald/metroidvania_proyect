using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [Header("Hover Settings")]
    public float floatSpeed = 2f;      // How fast it bobs up and down
    public float floatHeight = 0.5f;   // How high and low it goes

    // Store the original starting position of the key
    private Vector3 startPos;

    void Start()
    {
        // Remember exactly where we placed the key in the scene
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a Sine Wave
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Apply the new position to the key
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
