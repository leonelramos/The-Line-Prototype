using Godot;
using System;
using System.Collections.Generic;
public static class Math3D
{
	public static float VectorOrientation(Vector3 v)
	{
		return Mathf.Atan2(-v.x, -v.z);
	}
	
	public static Vector3 AngleAsVector(float angle)
	{
	  return new Vector3(-Mathf.Sin(angle), 0, -Mathf.Cos(angle));
	}
	
	public static Vector3 BruteForceCentroid(List<Vector3> points)
	{
		Vector3 sum = Vector3.Zero;
		foreach(Vector3 point in points) sum += point;
		return sum *= 1F/points.Count;
	}
}
