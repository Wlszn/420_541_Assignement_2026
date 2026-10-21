using UnityEngine;
using TMPro; // TMP_Text
 
// Shows the speed boost icon and seconds left while the boost is on.
public class PowerUpHUD : MonoBehaviour
{
    [SerializeField] private PlayerPowerUps powerUps;
    [SerializeField] private GameObject speedIndicator; // icon + time
    [SerializeField] private TMP_Text speedTimeText;
 
    private void Update()
    {
        float timeLeft = powerUps.SpeedTimeLeft;
 
        // Visible for the whole boost, hidden the rest of the time
        speedIndicator.SetActive(timeLeft > 0f);
 
        // CeilToInt rounds 6.2 up to 7, so we never show 0s too early
        speedTimeText.text = Mathf.CeilToInt(timeLeft) + "s";
    }
}
