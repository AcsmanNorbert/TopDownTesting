using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static Damage;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Data")]
    public float maxHealth = 10;
    public float currentHealth;
    EnemyNavMesh enemyNavMesh;
    bool isDead;

    [SerializeField] float moneyReward;

    public float Money
    {
        get => moneyReward;
        set => moneyReward = value;
    }

    private void Start()
    {
        enemyNavMesh = gameObject.GetComponent<EnemyNavMesh>();
        currentHealth = maxHealth;
    }

    public void DoDamage(float damage, Damage.DamageType dmgType, Transform damager)
    {
        if (isDead) return;
        currentHealth -= damage;

        DamageNumberHandler numberDisplay = GameManager.i.numberDisplay;
        if (numberDisplay != null)
            numberDisplay.SpawnDisplay(transform.position, damage, dmgType);

        //if (dmgType == Damage.DamageType.AreaOfEffect)
        //enemyNavMesh.ApplyForce(damager);
        if (currentHealth <= 0f)
        {
            Dies();
            isDead = true;
        }
        //                                        error
        //NullReferenceException: Object reference not set to an instance of an object
        //EnemyHealth.DoDamage(System.Single damage, Damage + DamageType dmgType, UnityEngine.Transform damager)(at Assets / Entities / EnemyHealth.cs:35)
        //SpellCasting.BurstCollision(UnityEngine.Collider[] explosionHit, UnityEngine.Transform transform, Damage damage, System.Collections.Generic.List`1[UnityEngine.Collider] & colliders)(at Assets / Spells / SpellCasting.cs:100)
        /*if(enemyNavMesh.currentState == EnemyNavMesh.State.Seeking)
            enemyNavMesh.TargetFoundAlert();*/
    }

    private void Dies()
    {
        MoneyManager.GainMoney(moneyReward * GameManager.i.roomManager.currentDifficulty);
        if (enemyNavMesh != null)
        {
            enemyNavMesh.SetState(EnemyNavMesh.State.Dead);
            Destroy(this);
        }
        else
            Destroy(gameObject);
    }
}
