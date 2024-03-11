using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    PlayerHealth playerHealth;
    [SerializeField] TMP_Text text;
    [SerializeField] Image image;
    Color textBaseColor;
    Color imageBaseColor;

    private void Start()
    {
        playerHealth = GameManager.i.player.GetComponent<PlayerHealth>();
        textBaseColor = text.color;
        imageBaseColor = image.color;
    }

    //should upgrade this to an event
    void Update()
    {
        text.text = Mathf.Clamp(playerHealth.currentHealth, 0, float.MaxValue) + "/" + playerHealth.maxHealth;
        image.fillAmount = Mathf.Clamp(playerHealth.currentHealth / playerHealth.maxHealth, 0, 1);
        if (playerHealth.currentInvulnTimer > 0)
        {
            text.color = Color.yellow;
            image.color = Color.white;
        }
        else
        {
            text.color = textBaseColor;
            image.color = imageBaseColor;
        }
    }
}
