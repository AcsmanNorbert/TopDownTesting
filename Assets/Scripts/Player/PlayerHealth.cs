using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Data")]
    public float maxHealth;
    public static float currentHealth { get; private set; }
    [SerializeField] float hitInvulnerabilityTimer;
    bool hitInvulnerable;
    public static bool playerInvulnerable { private set; get; }

    [SerializeField] bool invulnerabilityCheat;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (invulnerabilityCheat || PlayerMovement.isDashing || hitInvulnerable)        
            playerInvulnerable = true;        
        else
            playerInvulnerable = false;
    }

    public void DoDamage(float damage, Damage.DamageType dmgType, Transform damager)
    {
        if (playerInvulnerable) return;

        currentHealth -= damage;
        if (currentHealth <= 0f)
            GameManager.i.SetGameState(GameManager.GameState.Dead);
        if(hitInvulnerabilityTimer > 0)
            StartCoroutine(HitInvulnerability());
    }

    private IEnumerator HitInvulnerability()
    {
        hitInvulnerable = true;
        yield return new WaitForSeconds(hitInvulnerabilityTimer); 
        hitInvulnerable = false;
    }
}
