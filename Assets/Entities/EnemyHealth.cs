using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Data")]
    public float maxHealth;
    public float currentHealth;
    private IMovementAI movementAI;

    private void Start()
    {
        movementAI = gameObject.GetComponent<IMovementAI>();
        currentHealth = maxHealth;
    }

    public void Damage(float damage, Damage.DamageType dmgType, Transform damager)
    {
        currentHealth -= damage;
        GameManager.i.numberDisplay.SpawnDisplay(transform.position, damage, dmgType);
        /*if(dmgType == Damage.DamageType.AreaOfEffect)
            movementAI.ApplyForce(damager);*/
        if (currentHealth <= 0f)
            Dies();
    }

    private void Dies()
    {
        if (movementAI != null)
        {
            movementAI.SetState(IMovementAI.State.Dead);
            Destroy(this);
        }
        else
            Destroy(gameObject);
    }
}
