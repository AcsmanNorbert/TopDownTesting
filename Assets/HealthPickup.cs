using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float healthGain = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != GameManager.i.player) return;
        other.GetComponent<PlayerHealth>().GainHealth(healthGain);
        Destroy(gameObject);
    }
}
