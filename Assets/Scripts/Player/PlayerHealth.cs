using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private Rigidbody rb;

    [Header("Data")]
    public float maxHealth;
    public float currentHealth;
    public bool invulnarable;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void Damage(float damage, IDamageable.DMGType dmgType, Transform damager)
    {
        if (!invulnarable)
        {
            currentHealth -= damage;
            if (currentHealth <= 0f)
                StartCoroutine(Dies());
        }
    }

    private IEnumerator Dies()
    {
        rb.freezeRotation = false;
        rb.velocity += transform.forward;
        GameObject.Destroy(this);

        yield return new WaitForSeconds(10f);
        GameObject.Destroy(gameObject);
    }
}
