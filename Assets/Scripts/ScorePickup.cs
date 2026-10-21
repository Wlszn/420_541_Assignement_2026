using UnityEngine;

// A coin that adds points to the score when the player touches it.
public class ScorePickup : MonoBehaviour
{
    [SerializeField] private int points = 50;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Double the points while the score boost is on
        int multiplier = 1;
        PlayerPowerUps powerUps = other.GetComponent<PlayerPowerUps>();
        if (powerUps != null)
        {
            multiplier = powerUps.ScoreMultiplier;
        }

        GameManager.Instance.AddScore(points * multiplier);

        // PlayClipAtPoint keeps playing even though we destroy the coin next
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        Destroy(gameObject);
    }
}