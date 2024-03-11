using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Firewall : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect;

    [Header("Data")]
    [SerializeField] Damage damage;
    [SerializeField][Range(0, 10)] float duration = 5;
    [SerializeField] float ticksPerSecond = 2;
    float timer;

    [Space(3)]
    [SerializeField] LayerMask projectileMask;
    [SerializeField] bool blockProjectile;

    [SerializeField] bool drawGizmos;
    bool isPlaying;

    private void Start()
    {
        visualEffect.SetFloat("Width", damage.hitBounds.x);
        visualEffect.SetFloat("Depth", damage.hitBounds.z);
        visualEffect.SetFloat("Duration", duration);

        StartCoroutine(Lerp());
    }

    private void Update()
    {
        if (isPlaying)
            if (visualEffect.aliveParticleCount == 0)
                Destroy(gameObject);

        if (timer <= 0)
        {
            timer = 1 / ticksPerSecond;
            Debug.Log("sadge");
            SpellCasting.BoxBurstCollision(transform, damage);
        }
        else
            timer -= Time.deltaTime;

        if (!blockProjectile) return;

        Collider[] projectileColliders = Physics.OverlapBox(
            transform.position, new Vector3(damage.hitBounds.x, 4, 0.5f), transform.rotation, projectileMask);
        if (projectileColliders != null)
        {
            foreach (var collider in projectileColliders)
            {
                Destroy(collider.gameObject);
            }
        }
    }

    private IEnumerator Lerp()
    {
        yield return new WaitForSeconds(1f);
        isPlaying = true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Matrix4x4 gizmoMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = gizmoMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(damage.hitBounds.x, 4, damage.hitBounds.z));

            if (blockProjectile)
            {
                Gizmos.matrix = gizmoMatrix;
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(damage.hitBounds.x, 4, 0.5f));
            }
        }
    }
}
