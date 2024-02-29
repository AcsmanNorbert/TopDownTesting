using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject player;
    public TMP_Text text;
    Color baseColor;

    private void Start()
    {
        baseColor = text.color;
    }

    void Update()
    {
        text.text = PlayerHealth.currentHealth.ToString();
        if (PlayerHealth.playerInvulnerable)
            text.color = Color.yellow;
        else
            text.color = baseColor;
    }
}
