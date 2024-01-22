using UnityEngine;

public class DamageNumberHandler : MonoBehaviour
{
    public GameObject display;

    public void SpawnDisplay(Vector3 targetPosition, float damage, Damage.DamageType dmgType)
    {
        DamageNumberDisplay newDisplay = Instantiate(display).GetComponent<DamageNumberDisplay>();
        newDisplay.gameObject.transform.SetParent(gameObject.transform);
        newDisplay.gameObject.SetActive(true);
        newDisplay.SetDisplay(targetPosition, damage, dmgType);
    }
}
