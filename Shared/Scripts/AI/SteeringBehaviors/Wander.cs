using Godot;
using System;

namespace ODG.AI
{
  public class Wander : Face
  {
	private float wanderOffset, wanderRadius, wanderRate, maxAcceleration, wanderOrientation;
	public Wander(
		Kinematic principal, 
		Kinematic target, 
		float maxRotation, 
		float maxAngularAcceleration,
		float slowdownRadius,
		float targetRadius,
		float wanderOffset,
		float wanderRadius,
		float wanderRate,
		float maxAcceleration
		) : base(
		principal, 
		target, 
		maxRotation, 
		maxAngularAcceleration,
		slowdownRadius,
		targetRadius
		)
	{
		this.wanderOffset = wanderOffset;
		this.wanderRadius = wanderRadius;
		this.wanderRate = wanderRate;
		this.maxAcceleration = maxAcceleration;
	}
			
	public override SteeringOutput GetSteering()
	{
	  SteeringOutput output = new SteeringOutput();
	  float rand = Math.RandomBinomial();
	  wanderOrientation += rand * wanderRate;
	  Target.Orientation = wanderOrientation + Principal.Orientation;

	  Target.Position = Principal.Position + wanderOffset * Math3D.AngleAsVector(Principal.Orientation);

	  Target.Position += wanderRadius * Math3D.AngleAsVector(Target.Orientation);
	  
	  output = base.GetSteering();
	  output.linear = maxAcceleration * Math3D.AngleAsVector(Principal.Orientation);
	  
	  return output;
	}
  }
}
