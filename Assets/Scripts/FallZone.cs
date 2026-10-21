using UnityEngine;

// A big invisible trigger under the level. Falling into it restarts the level.
public class FallZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.RestartLevel();
    }
}