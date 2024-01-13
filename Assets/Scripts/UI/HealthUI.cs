using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject player;
    private PlayerHealth playerHealth;
    public TMP_Text text;

    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        text.text = playerHealth.currentHealth.ToString();
    }
}
