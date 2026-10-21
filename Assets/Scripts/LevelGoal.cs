using UnityEngine;

// The end goal of a level 
//Touching loads the next scene.
public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.SaveFinalScore();
        GameManager.Instance.LoadNextLevel();
    }
}