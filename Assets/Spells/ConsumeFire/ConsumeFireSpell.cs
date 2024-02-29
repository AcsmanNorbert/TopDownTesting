using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ConsumeFireSpell : MonoBehaviour
{
    [SerializeField] GameObject trailVFX;
    [SerializeField] VisualEffect explosionVisualEffect;
    [SerializeField] ParticleSystem ps;
    [SerializeField] float castDelay = 1f;
    [SerializeField] Damage firstDamage;
    [SerializeField] Damage secondDamage;
    [SerializeField] float extraDamage;
    [SerializeField] bool drawGizmos;

    bool isPlaying;

    public static event Action<float> OnExplosion;
    public static event Action<Transform> OnSpawnTrail;

    void Start()
    {
        StartCoroutine(DoAttack());
        ParticleSystem.MainModule main = ps.main;
        main.startLifetime = castDelay;
        main.startSize = firstDamage.hitRadius + 1;
        ps.Play();
    }

    private void Update()
    {
        if (isPlaying)
            if (explosionVisualEffect.aliveParticleCount <= 0 && ps.particleCount <= 0)
                Destroy(gameObject);        
    }

    private IEnumerator DoAttack()
    {
        OnExplosion = null;
        OnSpawnTrail = null;
        SpellCasting.SphereBurstCollision(transform, firstDamage, out List<Collider> colliders);
        if (colliders != null)
        {
            int multyplier = 0;
            foreach (var collider in colliders)
            {
                FireDOT onFire = collider.GetComponent<FireDOT>();
                if (onFire != null)
                {
                    Destroy(onFire);
                    multyplier++;
                    
                    GameObject newTrail = Instantiate(trailVFX);
                    newTrail.transform.position = collider.transform.position;
                }
            }
            yield return new WaitForSeconds(0.02f);
            OnSpawnTrail?.Invoke(transform.parent);
            if (multyplier > 0)
            {
                yield return new WaitForSeconds(castDelay);
                OnExplosion?.Invoke(secondDamage.hitRadius);
                secondDamage.baseDamage = secondDamage.baseDamage + extraDamage * multyplier;
                SpellCasting.SphereBurstCollision(transform, secondDamage);
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
            Gizmos.DrawWireSphere(transform.position, firstDamage.hitRadius);
            Gizmos.color = new Color(1f, 0f, 0f);
            Gizmos.DrawWireSphere(transform.position, secondDamage.hitRadius);
        }
    }
}
