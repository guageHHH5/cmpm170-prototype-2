using UnityEngine;

public class RespawnBlock : MonoBehaviour
{
    public Transform respawnPoint;
    private PlayerManager playerManager; 
    private void Start()
    {
        playerManager = FindFirstObjectByType<PlayerManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerManager.setSpawnpoint(respawnPoint);
        }
    }
}
