using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFlask : MonoBehaviour
{/*
    [SerializeField] GameObject flask;
    [SerializeField] float projectileSpeed;

    Vector3 castPosition;
    Vector3 mousePosition;
    List<Vector3> pathPoints = new();
    float startTimer;

    void Start()
    {
        castPosition = transform.position;
        startTimer = Time.time;

        mousePosition = PlayerInput.GetMousePosition(transform);
        mousePosition.y = 0;

        BallisticsTester.GetPathPoints(castPosition, mousePosition, out pathPoints);
    }

    private void Update()
    {
        float currentTimer = Time.time - startTimer;
        currentTimer = currentTimer * projectileSpeed;
        transform.position = Vector3.Lerp(transform.position, pathPoints[(int)currentTimer], Time.deltaTime * projectileSpeed);

        float currentDistance = Vector3.Distance(transform.position, mousePosition);
        if (currentDistance <= 0.2f)
        {
            Instantiate(flask, mousePosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }*/
}
