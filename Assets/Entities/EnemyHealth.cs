using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Data")]
    public float maxHealth;
    public float currentHealth;
    EnemyNavMesh enemyNavMesh;

    private void Start()
    {
        enemyNavMesh = gameObject.GetComponent<EnemyNavMesh>();
        currentHealth = maxHealth;
    }

    public void DoDamage(float damage, Damage.DamageType dmgType, Transform damager)
    {
        currentHealth -= damage;

        DamageNumberHandler numberDisplay = GameManager.i.numberDisplay;
        if (numberDisplay != null)
            numberDisplay.SpawnDisplay(transform.position, damage, dmgType);

        //if (dmgType == Damage.DamageType.AreaOfEffect)
        //enemyNavMesh.ApplyForce(damager);
        if (currentHealth <= 0f)
            Dies();
        enemyNavMesh.TargetFoundAlert();        
    }

    private void Dies()
    {
        if (enemyNavMesh != null)
        {
            enemyNavMesh.SetState(EnemyNavMesh.State.Dead);
            Destroy(this);
        }
        else
            Destroy(gameObject);
    }
}
