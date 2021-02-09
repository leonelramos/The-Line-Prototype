using Godot;
using System;
using System.Collections.Generic;
using ODG.Input;
using ODG.AI;

public class EnemyShip01 : KinematicBody, IFleetShip
{
	PlayableShipInputInterpreter inputInterpreter;
	
	Kinematic playerKinematic;
	Kinematic targetKinematic;
	List<IFleetShip> allyShips;
	List<IFleetShip> enemyShips;
	List<Kinematic> allyShipsKinematics;
	List<Kinematic> enemyShipsKinematics;
	
	Arrive arriveSteering;
	FaceForward faceForwardSteering;
	Seperation seperationSteering;
	CollisionAvoidance collisionAvoidanceSteering;
	bool printFlag = true;
	
	private bool connected = false;
	private bool allyShipsReady;
	private bool enemyShipsReady;

	public override void _Ready()
	{
		playerKinematic = new Kinematic(Translation, Rotation.y);
		targetKinematic = new Kinematic(Translation, Rotation.y);
		allyShipsKinematics = new List<Kinematic>();
		enemyShipsKinematics = new List<Kinematic>();
		allyShipsReady = false;
		enemyShipsReady = false;
	}
	
	public List<IFleetShip> EnemyFleet
	{
		set
		{
			enemyShips = value;
		}
	}

	public List<IFleetShip> AllyFleet
	{
		set
		{
			allyShips = value;
		}
	}
}
