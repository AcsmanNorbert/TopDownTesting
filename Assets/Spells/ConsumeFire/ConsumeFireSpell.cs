using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeFireSpell : MonoBehaviour
{
    //[SerializeField] VisualEffect visualEffect;
    [SerializeField] float castDelay = 1f;
    [SerializeField] Damage firstDamage;
    [SerializeField] Damage secondDamage;
    [SerializeField] bool drawGizmos;

    bool isPlaying;

    void Start()
    {
        StartCoroutine(DoAttack());
    }

    private void Update()
    {
        if (isPlaying)
            //if (visualEffect.aliveParticleCount == 0)
            Destroy(gameObject);
    }

    private IEnumerator DoAttack()
    {
        SpellCasting.SphereExplosion(transform, firstDamage, out List<Collider> colliders);
        
        if(colliders != null)
        {
            yield return new WaitForSeconds(castDelay);
            int multyplier = 0;
            foreach (var collider in colliders)
            {
                FireDOT onFire = collider.GetComponent<FireDOT>();
                if (onFire != null)
                    multyplier++;
            }
            if (multyplier > 0)
            {
                Damage multypliedDamage = secondDamage;
                multypliedDamage.explosionDamage = multypliedDamage.explosionDamage * multyplier;
                SpellCasting.SphereExplosion(transform, multypliedDamage);
            }
        }

        yield return new WaitForSeconds(1f);
        isPlaying = true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = new Color(0f, 0f, 1f);
            Gizmos.DrawWireSphere(transform.position, firstDamage.explosionRadius);
            Gizmos.color = new Color(1f, 0f, 0f);
            Gizmos.DrawWireSphere(transform.position, secondDamage.explosionRadius);
        }
    }
}
