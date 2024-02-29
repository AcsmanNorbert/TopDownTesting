using UnityEngine;
using UnityEngine.VFX;

public class KillVisualEffects : MonoBehaviour
{
    [SerializeField] VisualEffect[] visualEffects;
    [SerializeField] float minimumLifeTime = 1f;
    float lifeTime = 0f;

    private void Start()
    {
        if (visualEffects == null)
            visualEffects[0] = GetComponent<VisualEffect>();
    }

    private void Update()
    {
        if (visualEffects == null)
            return;
        lifeTime += Time.deltaTime;
        float particleCount = 0f;
        if (lifeTime > minimumLifeTime)
        {
            foreach (VisualEffect vfx in visualEffects)
                particleCount += vfx.aliveParticleCount;
            if (particleCount != 0) return;
            Destroy(gameObject);
        }
    }
}
