using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[ExecuteAlways]
public class BallisticsTester : MonoBehaviour
{
	public enum BalliscticPathType
	{
		Angle,
		StartSpeed,
	}

	[SerializeField] LineRenderer lineRenderer;
	[SerializeField] float gravity = 9.81f;

	[SerializeField] float angleDeg = 45f;
	[SerializeField] float startSpeed = 10f;
	[SerializeField] BalliscticPathType pathType = BalliscticPathType.Angle;
	[SerializeField] float time;
	[SerializeField] bool low = true;
	[SerializeField] float timeStep = 0.1f;

	readonly List<Vector3> path = new();
	[SerializeField] bool mousePos = false;
	[SerializeField] Transform target;
	[SerializeField] int steps;
	void Update()
	{
		Vector3 startPoint = transform.position;
		Vector3 targetPoint = new();

		if (mousePos)
			targetPoint = PlayerInput.GetMousePosition(transform);
		else
			targetPoint = target.position;

		Vector3 distanceVector = targetPoint - startPoint;
		float angleRad = angleDeg * Mathf.Deg2Rad;
		bool hasSolution = false;

		if (pathType == BalliscticPathType.Angle)		
			hasSolution = Ballistics.TryGetSpeed(distanceVector, gravity, angleRad, out startSpeed);		
		else if (pathType == BalliscticPathType.StartSpeed)		
			hasSolution = Ballistics.TryGetAngle(distanceVector, gravity, startSpeed, low, out angleRad);

		if (!hasSolution) return; 

		angleDeg = angleRad * Mathf.Rad2Deg; 
		Vector2 velocity2D = Ballistics.GetVelocity(startSpeed, angleRad);
		Vector3 velocity = velocity2D.ExtendTo3D(distanceVector);

		time = Ballistics.GetTime(distanceVector.GetHorizontalSize(), angleRad, startSpeed);		

		timeStep = time / steps;
		timeStep += timeStep / steps;
		Ballistics.GetPath(startPoint, velocity, gravity, time, timeStep, path);

		lineRenderer.positionCount = path.Count;
		lineRenderer.SetPositions(path.ToArray());
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		foreach (Vector3 point in path)
			Gizmos.DrawSphere(point, 0.1f);
	}

	public static bool GetPathPoints(Vector3 startPoint, Vector3 targetPoint, out List<Vector3> pathPoints)
	{
        float gravity = 9.81f;
        float angleDeg = 45f;
        float startSpeed = 10f;
        BalliscticPathType pathType = BalliscticPathType.Angle;
        bool low = true;
        float timeStep = 0.2f;

        Vector3 distanceVector = targetPoint - startPoint;
        float angleRad = angleDeg * Mathf.Deg2Rad;
        bool hasSolution = false;

        if (pathType == BalliscticPathType.Angle)
            hasSolution = Ballistics.TryGetSpeed(distanceVector, gravity, angleRad, out startSpeed);
        else if (pathType == BalliscticPathType.StartSpeed)
            hasSolution = Ballistics.TryGetAngle(distanceVector, gravity, startSpeed, low, out angleRad);

		if (!hasSolution)
		{
			pathPoints = null;
			return hasSolution; 
		}

        angleDeg = angleRad * Mathf.Rad2Deg;
        Vector2 velocity2D = Ballistics.GetVelocity(startSpeed, angleRad);
        Vector3 velocity = velocity2D.ExtendTo3D(distanceVector);

		float time = 1;

		pathPoints = new();
        Ballistics.GetPath(startPoint, velocity, gravity, time, timeStep, pathPoints);

		return hasSolution;
    }
}