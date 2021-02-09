using Godot;
using System;
using System.Collections.Generic;

public class FleetManager : Node
{
	List<IFleetShip> allyFleetShips;
	List<IFleetShip> enemyFleetShips;
	
	public override void _Ready()
	{
		allyFleetShips = new List<IFleetShip>();
		enemyFleetShips = new List<IFleetShip>();
		
		var playerFleet = GetNode<AllyFleet>("AllyFleet");
		var playerFleetSize = playerFleet.GetChildCount();
		for(int i = 0; i < playerFleetSize; i++)
			allyFleetShips.Add(playerFleet.GetChild<IFleetShip>(i));
			
		var enemyFleet = GetNode<EnemyFleet>("EnemyFleet");
		var enemyFleetSize = enemyFleet.GetChildCount();
		for(int i = 0; i < enemyFleetSize; i++)
			enemyFleetShips.Add(enemyFleet.GetChild<IFleetShip>(i));
		playerFleet.InjectEnemyFleet(enemyFleetShips);
	}
	
	public List<T> GetAllyFleetShips<T>()
	{
		List<T> castedShips = new List<T>();
		foreach(IFleetShip ship in allyFleetShips) castedShips.Add((T)ship);
		return castedShips;
	}
	
	public List<T> GetEnemyFleetShips<T>()
	{
		List<T> castedShips = new List<T>();
		foreach(IFleetShip ship in enemyFleetShips) castedShips.Add((T)ship);
		return castedShips;
	}
}
