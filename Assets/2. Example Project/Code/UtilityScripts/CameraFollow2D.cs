using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;       // Reference to the player's transform.
    public float smoothSpeed = 0.125f;  // Smooth damp time (seconds). Set to 0 for a rigid lock.
    public Vector3 offset;         // Offset between the camera and the player.

    Vector3 smoothVelocity;

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned to the CameraFollow2D script.");
            return;
        }

        Vector3 desiredPosition = player.position + offset;

        if (smoothSpeed <= 0f)
        {
            transform.position = desiredPosition;
            return;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref smoothVelocity,
            smoothSpeed);
    }
}
