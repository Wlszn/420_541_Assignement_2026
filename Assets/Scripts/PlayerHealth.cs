using UnityEngine;
using UnityEngine.SceneManagement;

/// Manages the player's health and handles the game over state.
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float restartDelay = 2f;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage = 1)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;

        if (CurrentHealth == 0)
        {
            Invoke(nameof(GameOver), restartDelay);
        }
    }

    private void GameOver()
    {
        GameManager.Instance.RestartLevel();
    }
}