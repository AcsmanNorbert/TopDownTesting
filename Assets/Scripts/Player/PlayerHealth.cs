using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Data")]
    public float maxHealth;
    public float currentHealth { get; private set; }
    public float currentInvulnTimer { get; private set; }

    [SerializeField] float hitInvulnTimer;
    [SerializeField] float genericInvulnTimer;
    [SerializeField] bool invulnerabilityCheat; 

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentInvulnTimer > 0)
            currentInvulnTimer -= Time.deltaTime;
    }

    public void DoDamage(float damage, Damage.DamageType dmgType, Transform damager)
    {
        if (invulnerabilityCheat) return;
        if (currentInvulnTimer > 0) return;

        currentHealth -= damage;
        if (currentHealth <= 0f)
            GameManager.i.SetGameState(GameManager.GameState.Dead);

        MakeInvulnerable(hitInvulnTimer);
    }

    public void GainHealth(float health)
    {
        maxHealth += health;
        currentHealth = maxHealth;
    }

    public void MakeInvulnerable(float duration)
    {
        if(currentInvulnTimer < duration)
            currentInvulnTimer = duration;
    }
}
