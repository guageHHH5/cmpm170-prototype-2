using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform spawnPoint; // Reference to the spawn point
    private Rigidbody2D playerRb; // Reference to the player's Rigidbody2D component

    private void Start()
    {
        playerRb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Reset"))
        {
            // Reset the player's velocity
            playerRb.linearVelocity = Vector2.zero;

            // Teleport the player to the spawn point position
            playerRb.transform.position = spawnPoint.position;
        }
    }

    public void setSpawnpoint(Transform newSpawnPoint){
        spawnPoint = newSpawnPoint;
    }
}
