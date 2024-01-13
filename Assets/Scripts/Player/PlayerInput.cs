using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput
{
    public static Vector3 GetMousePosition(Transform transform)
    {
        Vector3 distanceFromCamera = transform.position;
        Plane plane = new Plane(transform.up, distanceFromCamera);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float enter = 0.0f;
        if (plane.Raycast(ray, out enter))
            return ray.GetPoint(enter);
        else
            return transform.forward;
    }
}
