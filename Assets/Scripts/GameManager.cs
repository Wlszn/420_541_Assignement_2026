using UnityEngine;
using UnityEngine.SceneManagement;

// The Game Manager: a singleton that survives scene changes.
// It keeps the score and loads scenes.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Static, so the End Screen can still read it
    public static int FinalScore { get; private set; }

    public int Score { get; private set; }

    [SerializeField] private string firstLevelScene = "Level1";

    private int scoreAtLevelStart;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void SaveFinalScore()
    {
        FinalScore = Score;
    }

    public void StartNewGame()
    {
        Score = 0;
        scoreAtLevelStart = 0;
        SceneManager.LoadScene(firstLevelScene);
    }

    public void LoadNextLevel()
    {
        scoreAtLevelStart = Score;   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        Score = scoreAtLevelStart;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}