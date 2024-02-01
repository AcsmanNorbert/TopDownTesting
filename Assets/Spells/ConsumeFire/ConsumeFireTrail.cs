using UnityEngine;

public class ConsumeFireTrail : MonoBehaviour
{
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] float speed = 0.1f;

    Vector3 startingPosition;
    Transform targetTransform;
    bool isPlaying;
    float lerpTimer;

    private void Start()
    {
        ConsumeFireSpell.OnSpawnTrail += ConsumeFireSpell_OnSpawnTrail;
    }

    private void ConsumeFireSpell_OnSpawnTrail(Transform target)
    {
        startingPosition = transform.position;
        targetTransform = target;
        isPlaying = true;
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (lerpTimer < 1f)
            {
                transform.position = Vector3.Lerp(startingPosition, targetTransform.position, lerpTimer);
                trailRenderer.time = 2 - lerpTimer * 2;
                lerpTimer += speed * Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetTransform.position);
        }
    }
}
