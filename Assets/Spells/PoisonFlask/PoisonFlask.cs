using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFlask : MonoBehaviour
{
    Vector3 castPosition;
    Vector3 mousePosition;
    [SerializeField]GameObject flask;
    [SerializeField] float projectileSpeed;

    void Start()
    {
        castPosition = transform.position;

        mousePosition = PlayerInput.GetMousePosition(transform);
        mousePosition.y = 0;

        transform.LookAt(mousePosition);

        GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
    }

    private void Update()
    {
        float currentDistance = Vector3.Distance(transform.position, mousePosition);
        if (currentDistance <= 0.2f)
        {
            Instantiate(flask, mousePosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
