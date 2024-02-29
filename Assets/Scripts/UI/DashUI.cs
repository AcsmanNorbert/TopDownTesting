using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashUI : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Image sprite;
    [SerializeField] Image cooldownSprites;
    [SerializeField] TMP_Text numberText;

    private void Update()
    {
        float cooldown = playerMovement.dashCooldown;
        float currentCooldown = playerMovement.dashCurrentCooldown;
        bool canDash = playerMovement.canDash;
        if (currentCooldown > 0)
        {
            numberText.gameObject.SetActive(true);
            string text = ((int)(currentCooldown * 10) / 10f).ToString();
            numberText.text = text;
            cooldownSprites.fillAmount = currentCooldown / cooldown;
        }
        else
        {
            numberText.gameObject.SetActive(false);
            cooldownSprites.fillAmount = 0;
        }
        sprite.color = canDash ? Color.white : Color.grey;
    }
}
