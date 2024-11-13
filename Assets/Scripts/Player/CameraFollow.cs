using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float followThreshold = 2.0f; // Distance from the camera at which it starts following the player
    public float smoothTime = 0.3f; // Time taken to smooth the camera movement

    private Vector3 velocity = Vector3.zero; // Reference velocity for SmoothDamp

    private void FixedUpdate()
    {
        // Calculate the distance between the camera and the player on the x-axis
        float distanceFromPlayer = player.position.x - transform.position.x;

        // Check if the player has moved beyond the follow threshold
        if (Mathf.Abs(distanceFromPlayer) > followThreshold)
        {
            // Calculate the target position (only move on the x-axis)
            float targetX = player.position.x - Mathf.Sign(distanceFromPlayer) * followThreshold;
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

            // Smoothly move the camera to the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
