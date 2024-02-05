using UnityEngine;

public class DrawCollider : MonoBehaviour
{
    [SerializeField] Color color = Color.red;

    private void OnDrawGizmos()
    {
        if (TryGetComponent<Collider>(out Collider collider))
        {
            Gizmos.color = color;
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                Vector3 center = new Vector3(
                    boxCollider.center.x * transform.lossyScale.x,
                    boxCollider.center.y * transform.lossyScale.y,
                    boxCollider.center.z * transform.lossyScale.z);
                Matrix4x4 gizmoMatrix = Matrix4x4.TRS(
                    transform.position + center, transform.rotation, transform.lossyScale);
                Gizmos.matrix = gizmoMatrix;
                Gizmos.DrawWireCube(Vector3.zero, boxCollider.size);
                return;
            }
            SphereCollider sphereCollider = GetComponent<SphereCollider>();
            if (sphereCollider != null)
            {
                float biggestSide = CheckBiggestSideOfVector3(transform.lossyScale);
                Vector3 scale = new(biggestSide, biggestSide, biggestSide);
                Vector3 center = new Vector3(
                    sphereCollider.center.x * transform.lossyScale.x,
                    sphereCollider.center.y * transform.lossyScale.y,
                    sphereCollider.center.z * transform.lossyScale.z);
                Matrix4x4 gizmoMatrix = Matrix4x4.TRS(
                    transform.position + center, transform.rotation, scale);
                Gizmos.matrix = gizmoMatrix;
                Gizmos.DrawWireSphere(Vector3.zero, sphereCollider.radius);
                return;
            }
        }
    }

    private float CheckBiggestSideOfVector3(Vector3 vector)
    {
        float biggest = vector.x;
        if (biggest < vector.y)
            biggest = vector.y;
        if (biggest < vector.z)
            biggest = vector.z;
        return biggest;
    }
}
