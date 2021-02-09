using Godot;
using System;
using System.Collections.Generic;

public class SceneSetup : Spatial
{
	List<Vector3> allyShipLocations;
	List<Spatial> allyShips;
	public override void _Ready()
	{
		allyShips = GetNode<FleetManager>("FleetManager").GetAllyFleetShips<Spatial>();
		allyShipLocations = new List<Vector3>();
	}
	
	public override void _PhysicsProcess(float _delta)
	{
		allyShipLocations.Clear();
		foreach(Spatial ship in allyShips) allyShipLocations.Add(ship.GlobalTransform.origin);
		GetNode("Camera").Call("set_look_at_point", Math3D.BruteForceCentroid(allyShipLocations));
	}
}
