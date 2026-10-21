using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{

    // The speed multiplier for the player's boost power-up
    [SerializeField] private float boostMultiplier = 1.5f;

    // The duration of the boost power-up in seconds
    public float SpeedTimeLeft {get; private set; }
    public float ScoreTimeLeft {get; private set; }
    public float JumpTimeLeft {get; private set; }

    // Multiplier is 2 when pickup otherwise 1
    public int ScoreMultiplier => ScoreTimeLeft > 0f ? 2 : 1;

    private PlayerMovement movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        
    }

    //Called by pickup when the player collects a speed boost power-up
    public void ActivateSpeedBoost(float duration)
    {
        SpeedTimeLeft = duration;
        movement.SpeedMultiplier = boostMultiplier;
    }

    public void ActivateScoreBoost(float duration)
    {
        ScoreTimeLeft = duration;
    }

    public void ActivateDoubleJump(float duration)
    {
        JumpTimeLeft = duration;
        movement.CanDoubleJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Speed Boost
        if (SpeedTimeLeft > 0f)
        {
            SpeedTimeLeft -= Time.deltaTime;
            if (SpeedTimeLeft <= 0f)
            {
                SpeedTimeLeft = 0f;
                movement.SpeedMultiplier = 1f;

            }
        }

        //Score Boost
        if (ScoreTimeLeft > 0f)
        {
            ScoreTimeLeft -= Time.deltaTime;
            if (ScoreTimeLeft < 0f) ScoreTimeLeft = 0f;
        }

        //Jump Boost
        if (JumpTimeLeft > 0f)
        {
            JumpTimeLeft -= Time.deltaTime;
            if (JumpTimeLeft <= 0f)
            {
                JumpTimeLeft = 0f;
                movement.CanDoubleJump = false;
            }
        }
    }
}
