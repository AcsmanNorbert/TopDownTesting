
using System.Collections.Generic;
using UnityEngine;

static class Ballistics
{
    public static bool TryGetVelocity(Vector2 startPoint, Vector2 targetPoint, float gravity, float angleRad, out float speed) =>
        TryGetSpeed(targetPoint - startPoint, gravity, angleRad, out speed);
    public static bool TryGetVelocity(Vector3 startPoint, Vector3 targetPoint, float gravity, float angleRad, out float speed) =>
        TryGetSpeed(targetPoint - startPoint, gravity, angleRad, out speed);
    public static bool TryGetSpeed(Vector3 distanceVector, float gravity, float angleRad, out float speed) =>
        TryGetSpeed(distanceVector.FlattenTo2D(), gravity, angleRad, out speed);

    public static bool TryGetSpeed(Vector2 distanceVector, float gravity, float angleRad, out float speed)
    {
        float dy = distanceVector.y;
        float dx = distanceVector.x;

        float cosAlpha = Mathf.Cos(angleRad);
        float tanAlpha = Mathf.Tan(angleRad);

        float root = gravity * dx * dx / (2 * cosAlpha * cosAlpha * (dx * tanAlpha - dy));
        if (root < 0)
        {
            speed = 0;
            return false;
        }
        else
        {
            speed = Mathf.Sqrt(root);
            return root > 0;
        }
    }

    public static bool TryGetAngle(Vector2 startPoint, Vector2 targetPoint, float gravity, float speed, bool low, out float angleRad) =>
        TryGetAngle(targetPoint - startPoint, gravity, speed, low, out angleRad);
    public static bool TryGetAngle(Vector3 startPoint, Vector3 targetPoint, float gravity, float speed, bool low, out float angleRad) =>
        TryGetAngle(targetPoint - startPoint, gravity, speed, low, out angleRad);
    public static bool TryGetAngle(Vector3 distanceVector, float gravity, float speed, bool low, out float angleRad) =>
        TryGetAngle(distanceVector.FlattenTo2D(), gravity, speed, low, out angleRad);

    public static bool TryGetAngle(Vector2 distanceVector, float gravity, float speed, bool low, out float angleRad)
    {
        float dy = distanceVector.y;
        float dx = distanceVector.x;

        float v2 = speed * speed;
        float v4 = v2 * v2;
        float x2 = dx * dx;
        float underRoot = v4 - gravity * (gravity * x2 + 2 * dy * v2);

        if (underRoot < 0)
        {
            angleRad = 0;
            return false;
        }

        float root = Mathf.Sqrt(underRoot);

        if (low)
            angleRad = Mathf.Atan((v2 - root) / (gravity * dx));
        else
            angleRad = Mathf.Atan((v2 + root) / (gravity * dx));

        return true;
    }

    // Utility Functions: Vectors, Vertical Angles -------------------------------------------

    public static float GetVerticalAngleRad(this Vector2 velocity) => Mathf.Atan(velocity.y / velocity.x);
    public static float GetVerticalAngleRad(this Vector3 velocity) => Mathf.Atan(velocity.y / velocity.GetHorizontalSize());
    public static Vector2 FlattenTo2D(this Vector3 v3) => new(v3.GetHorizontalSize(), v3.y);
    public static Vector3 ExtendTo3D(this Vector2 v2, Vector3 horizontalDirection)
    {
        Vector2 distanceVectorH = new(horizontalDirection.x, horizontalDirection.z);
        Vector2 directionH = distanceVectorH.normalized;
        return new(directionH.x * v2.x, v2.y, directionH.y * v2.x);
    }
    public static float GetHorizontalSize(this Vector3 v) => new Vector2(v.x, v.z).magnitude;

    // GetTime -------------------------------------------

    public static float GetTime(float distanceX, float angleRad, float speed)
    {
        float vx = speed * Mathf.Cos(angleRad);
        return Mathf.Abs(distanceX) / vx;
    }

    public static float GetTime(Vector2 distanceVector, Vector2 velocity) =>
        GetTime(distanceVector.x, velocity.GetVerticalAngleRad(), velocity.y);

    public static float GetTime(Vector3 distanceVector, Vector3 velocity)
    {
        float dx = distanceVector.GetHorizontalSize();
        float angleRad = Mathf.Atan(velocity.y / dx);
        return GetTime(dx, angleRad, velocity.y);
    }

    // GetVelocity -------------------------------------------

    public static bool TryGetVelocity(Vector2 startPoint, Vector2 targetPoint, float gravity, float angleRad, out Vector2 velocity) =>
        TryGetVelocity(targetPoint - startPoint, gravity, angleRad, out velocity);

    public static bool TryGetVelocity(Vector2 distanceVector, float gravity, float angleRad, out Vector2 velocity)
    {
        if (!TryGetSpeed(distanceVector, gravity, angleRad, out float speed))
        {
            velocity = Vector2.zero;
            return false;
        }

        float vx = speed * Mathf.Cos(angleRad);
        float vy = speed * Mathf.Sin(angleRad);
        velocity = new(vx, vy);
        return true;
    }

    public static bool TryGetVelocity(Vector2 startPoint, Vector2 targetPoint, float gravity, float speed, bool low, out Vector2 velocity) =>
        TryGetVelocity(targetPoint - startPoint, gravity, speed, low, out velocity);

    public static bool TryGetVelocity(Vector2 distanceVector, float gravity, float speed, bool low, out Vector2 velocity)
    {
        if (!TryGetAngle(distanceVector, gravity, speed, low, out float angleRad))
        {
            velocity = Vector2.zero;
            return false;
        }

        velocity = GetVelocity(speed, angleRad);
        return true;
    }

    public static Vector2 GetVelocity(float speed, float angleRad)
    {
        float vx = speed * Mathf.Cos(angleRad);
        float vy = speed * Mathf.Sin(angleRad);
        return new(vx, vy);
    }


    // GetVelocity 3D -------------------------------------------

    public static bool TryGetVelocity(Vector3 startPoint, Vector3 targetPoint, float gravity, float angleRad, out Vector3 velocity) =>
            TryGetVelocity(targetPoint - startPoint, gravity, angleRad, out velocity);

    public static bool TryGetVelocity(Vector3 distanceVector, float gravity, float angleRad, out Vector3 velocity)
    {
        Vector2 distanceVector2D = distanceVector.FlattenTo2D();
        if (!TryGetVelocity(distanceVector2D, gravity, angleRad, out Vector2 v2D))
        {
            velocity = Vector3.zero;
            return false;
        }

        Vector3 horizontalDirection = new Vector3(distanceVector.x, 0, distanceVector.z).normalized;
        velocity = new(horizontalDirection.x * v2D.x, v2D.y, horizontalDirection.z * v2D.x);
        return true;
    }

    public static bool TryGetVelocity(Vector3 startPoint, Vector3 targetPoint, float gravity, float speed, bool low, out Vector3 velocity) =>
        TryGetVelocity(targetPoint - startPoint, gravity, speed, low, out velocity);

    public static bool TryGetVelocity(Vector3 distanceVector, float gravity, float speed, bool low, out Vector3 velocity)
    {
        Vector2 distanceVector2D = distanceVector.FlattenTo2D();

        if (!TryGetVelocity(distanceVector2D, gravity, speed, low, out Vector2 v2D))
        {
            velocity = Vector3.zero;
            return false;
        }

        Vector3 horizontalDirection = new Vector3(distanceVector.x, 0, distanceVector.z).normalized;
        velocity = new(horizontalDirection.x * v2D.x, v2D.y, horizontalDirection.z * v2D.x);
        return true;
    }

    // Path Calculation, No Allocation  -------------------------------------------

    public static void GetPath(Vector2 startPoint, Vector2 startVelocity, float gravity, float fullTime, float timeStep, List<Vector2> path)
    {
        if (timeStep <= 0) return;
        path.Clear();
        path.Add(startPoint);
        Vector2 currentPoint = startPoint;
        Vector2 currentVelocity = startVelocity;
        float currentTime = 0;
        for (int i = 0; currentTime < fullTime; i++)
        {
            currentPoint += currentVelocity * timeStep;
            path.Add(currentPoint);
            currentVelocity += gravity * timeStep * Vector2.down;
            currentTime += timeStep;
        }
        float lastStep = fullTime - currentTime;
        currentPoint += currentVelocity * lastStep;
        path.Add(currentPoint);
    }

    public static void GetPath(Vector3 startPoint, Vector3 startVelocity, float gravity, float fullTime, float timeStep, List<Vector3> path)
    {
        if (timeStep <= 0) return;
        path.Clear();
        path.Add(startPoint);
        Vector3 currentPoint = startPoint;
        Vector3 currentVelocity = startVelocity;
        float currentTime = 0;
        for (int i = 0; currentTime < fullTime; i++)
        {
            currentPoint += currentVelocity * timeStep;
            path.Add(currentPoint);
            currentVelocity += gravity * timeStep * Vector3.down;
            currentTime += timeStep;
        }

        float lastStep = fullTime - currentTime;
        currentPoint += currentVelocity * lastStep;
        path.Add(currentPoint);

    }

    public static void GetPath(Vector2 startPoint, Vector2 startVelocity, float gravity, float fullTime, float timeStep, float drag, List<Vector2> path)
    {
        if (timeStep <= 0) return;
        path.Clear();
        path.Add(startPoint);
        Vector2 currentPoint = startPoint;
        Vector2 currentVelocity = startVelocity;
        float currentTime = 0;
        for (int i = 0; currentTime < fullTime; i++)
        {
            currentPoint += currentVelocity * timeStep;
            path.Add(currentPoint);
            currentVelocity += gravity * timeStep * Vector2.down;
            currentVelocity *= 1 - drag * timeStep;
            currentTime += timeStep;
        }
        float lastStep = fullTime - currentTime;
        currentPoint += currentVelocity * lastStep;
        path.Add(currentPoint);
    }

    public static void GetPath(Vector3 startPoint, Vector3 startVelocity, float gravity, float fullTime, float timeStep, float drag, List<Vector3> path)
    {
        if (timeStep <= 0) return;
        path.Clear();
        path.Add(startPoint);
        Vector3 currentPoint = startPoint;
        Vector3 currentVelocity = startVelocity;
        float currentTime = 0;
        for (int i = 0; currentTime < fullTime; i++)
        {
            currentPoint += currentVelocity * timeStep;
            path.Add(currentPoint);
            currentVelocity += gravity * timeStep * Vector3.down;
            currentVelocity *= 1 - drag * timeStep;
            currentTime += timeStep;
        }
        float lastStep = fullTime - currentTime;
        currentPoint += currentVelocity * lastStep;
        path.Add(currentPoint);
    }


}