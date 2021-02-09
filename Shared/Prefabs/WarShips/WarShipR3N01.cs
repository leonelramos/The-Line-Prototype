using Godot;
using System;
using System.Collections.Generic;
using ODG.Input;
using ODG.AI;

public class WarShipR3N01 : KinematicBody, IFleetShip
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
	
	// Arrive params
	float maxSpeed = 5;
	float maxAcceleration = .5F;
	float arriveSlowdownRadius = 5;
	float arriveTargetRadius = 1F;
	// FaceForward params
	float maxRotation = Mathf.Pi/4F;
	float maxAngularAcceleration = Mathf.Pi / 8F;
	float faceSlowdownRadius = Mathf.Pi / 20F;
	float faceTargetRadius = Mathf.Pi / 180F;
	// Seperation params
	float decay = 200F;
	float threshold = 15.0F;

	// .25 of a degree
	const float angleEpsilon = 0.004363323F;

	public override void _Ready()
	{
		playerKinematic = new Kinematic(Translation, Rotation.y);
		targetKinematic = new Kinematic(Translation, Rotation.y);
		allyShipsKinematics = new List<Kinematic>();
		enemyShipsKinematics = new List<Kinematic>();
		
		arriveSteering = new Arrive(
			playerKinematic, 
			targetKinematic, 
			maxSpeed, 
			maxAcceleration,
			arriveSlowdownRadius, 
			arriveTargetRadius
		);
			
		faceForwardSteering = new FaceForward(
			playerKinematic, 
			targetKinematic, 
			maxRotation, 
			maxAngularAcceleration,
			faceSlowdownRadius, 
			faceTargetRadius
		);
			
		inputInterpreter = new PlayableShipInputInterpreter(this);
		AddChild(inputInterpreter);
		allyShipsReady = false;
		enemyShipsReady = false;
	}
	
	public override void _Process(float delta){}
	
	public override void _PhysicsProcess(float delta)
	{
		
		UpdateTarget();
		
		Vector3 temp = targetKinematic.Position - Transform.origin;
		//temp = temp.Normalized();
		if(printFlag)
		{
			GD.Print("Pos : " + Translation);
			GD.Print("Glob : " + Transform.origin);
			GD.Print("Rot.y : " + Rotation.y);
			GD.Print("my func : " + Math3D.VectorOrientation(-Transform.basis.z));
			GD.Print("go to angle : " + Math3D.VectorOrientation(temp));
			printFlag = false;
		}
		
		var steeringForce = GetSteeringForces();
		ApplySteering(steeringForce, delta);
		var r = Math.MapToRangePi(Rotation.y);
		//if(steeringForce.angular < angleEpsilon)
		//{
			//GD.Print(steeringForce.angular);
		//}

		Translation = playerKinematic.Position;
		Vector3 newRotation = Rotation;
		newRotation.y = playerKinematic.Orientation;
		Rotation = newRotation;

		UpdateSteeringBehaviors();
	}

	public void ApplySteering(SteeringOutput steeringForce, float delta)
	{
		if(steeringForce.linear.Length() > maxAcceleration)
			steeringForce.linear = steeringForce.linear.Normalized() * maxAcceleration;
		if(steeringForce.angular > maxAngularAcceleration)
		{
			steeringForce.angular /= Mathf.Abs(steeringForce.angular);
			steeringForce.angular *= maxAngularAcceleration;
		}
		// fix floating point errors. Make this internal later
		if(Mathf.Abs(steeringForce.angular) < angleEpsilon) steeringForce.angular = 0;
		if(Mathf.Abs(steeringForce.linear.Length()) < Mathf.Epsilon) steeringForce.linear = Vector3.Zero;
		
		playerKinematic.Velocity += steeringForce.linear;
		playerKinematic.Rotation += steeringForce.angular;
		if(playerKinematic.Velocity.Length() > maxSpeed)
			playerKinematic.Velocity = playerKinematic.Velocity.Normalized() * maxSpeed;
		if(playerKinematic.Rotation > maxRotation)
		{
			playerKinematic.Rotation /= Mathf.Abs(playerKinematic.Rotation);
			playerKinematic.Rotation *= maxRotation;
		}
		// fix floating point errors. Make this internal later
		if(Mathf.Abs(playerKinematic.Rotation) < angleEpsilon) playerKinematic.Rotation = 0;
		if(Mathf.Abs(playerKinematic.Velocity.Length()) < Mathf.Epsilon) playerKinematic.Velocity = Vector3.Zero;
		playerKinematic.Position += playerKinematic.Velocity * delta;
		playerKinematic.Orientation += playerKinematic.Rotation * delta;
	}

	public SteeringOutput GetSteeringForces()
	{
		SteeringOutput steer = new SteeringOutput();
		steer += arriveSteering.GetSteering();
		if (allyShipsReady) steer += seperationSteering.GetSteering();
		//if(enemyShipsReady) steer += collisionAvoidanceSteering.GetSteering();
		steer += faceForwardSteering.GetSteering();
		return steer;
	}

	public void UpdateSteeringBehaviors()
	{
		arriveSteering.Principal = playerKinematic;
		arriveSteering.Target = targetKinematic;
		seperationSteering.Principal = playerKinematic;
		UpdateSeperationTargets();
		faceForwardSteering.Principal = playerKinematic;
	}
	
	public void UpdateTarget()
	{
		if(inputInterpreter.IsTargetSet())
		{
			targetKinematic.Position = inputInterpreter.GetTarget();
			printFlag = true;
		}
	}
	
	private void UpdateSeperationTargets()
	{
		for(int i = 0; i < allyShips.Count; i++)
		{
			allyShipsKinematics[i].Position = (allyShips[i] as Spatial).Translation;
			allyShipsKinematics[i].Orientation = (allyShips[i] as Spatial).Rotation.y;
		}
		seperationSteering.Targets = allyShipsKinematics;
	}
	
	public List<IFleetShip> EnemyFleet
	{
		set
		{
			enemyShips = value;
			foreach(IFleetShip ship in enemyShips)
			{
				enemyShipsKinematics.Add(new Kinematic((ship as Spatial).Translation, (ship as Spatial).Rotation.y));
			}
			collisionAvoidanceSteering = new CollisionAvoidance(playerKinematic, enemyShipsKinematics, 10, 10);
			enemyShipsReady = true;
		}
	}

	public List<IFleetShip> AllyFleet
	{
		set
		{
			allyShips = value;
			foreach(IFleetShip ship in allyShips)
			{
				allyShipsKinematics.Add(new Kinematic((ship as Spatial).Translation, (ship as Spatial).Rotation.y));
			}
			allyShipsReady = true;
			seperationSteering = new Seperation(playerKinematic, maxAcceleration, decay, threshold, allyShipsKinematics);
		}
	}
}
