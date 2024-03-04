using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    PlayerHealth playerHealth;
    public TMP_Text text;
    Color baseColor;

    private void Start()
    {
        playerHealth = GameManager.i.player.GetComponent<PlayerHealth>();
        baseColor = text.color;
    }

    void Update()
    {
        text.text = playerHealth.currentHealth.ToString();
        if (playerHealth.currentInvulnTimer > 0)
            text.color = Color.yellow;
        else
            text.color = baseColor;
    }
}
