using Godot;
using System;
using System.Collections.Generic;

public class AllyFleet : Node, IFleet
{
	List<IFleetShip> allyFleetShips;
	
	public override void _Ready()
	{
		allyFleetShips = new List<IFleetShip>();
		int fleetSize = GetChildCount();
		for(int i = 0; i < fleetSize; i++)
			allyFleetShips.Add(GetChild<IFleetShip>(i));
		InterconnectAllyFleet();
	}
	/*
	* Give every ship in the players fleet a reference to every other ship
	* in the players fleet. Required for group steering behaviors.
	*/
	private void InterconnectAllyFleet()
	{
		foreach(IFleetShip ship in allyFleetShips)
		{
			var allyShips = new List<IFleetShip>();
			foreach(IFleetShip allyShip in allyFleetShips)
				if(ship != allyShip) allyShips.Add(allyShip);
			ship.AllyFleet = allyShips;
		}
	}
	
	public void InjectEnemyFleet(List<IFleetShip> enemyFleetShips)
	{
		foreach(IFleetShip ship in allyFleetShips)
		{
			ship.EnemyFleet = enemyFleetShips;
		}
	}
}
