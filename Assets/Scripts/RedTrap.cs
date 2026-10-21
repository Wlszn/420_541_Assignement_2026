using UnityEngine;

// Red Trap that removes health from the player when they collide with it
public class RedTrap : MonoBehaviour
{
    [SerializeField] private int damage = 1; // Amount of health to remove from the player

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // Check if the colliding object is the player

        PlayerHealth health = other.GetComponentInParent<PlayerHealth>(); // Get the PlayerHealth component from the player
        if (health != null)
        {
            health.TakeDamage(damage); // Remove health from the player
        }
    }
}
