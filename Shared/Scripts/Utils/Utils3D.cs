using Godot;
using System;

public static class Utils3D
{
	public static Godot.Collections.Dictionary WorldQueryFromScreenPosition(Vector2 screenPosition, World world, Camera camera)
	{
		Vector3 rayOrigin = camera.ProjectRayOrigin(screenPosition);
		Vector3 rayEnd = rayOrigin + camera.ProjectRayNormal(screenPosition) * 1000;
		var spaceState = world.DirectSpaceState;
		var result = spaceState.IntersectRay(rayOrigin, rayEnd);
		return result;
	}
}

