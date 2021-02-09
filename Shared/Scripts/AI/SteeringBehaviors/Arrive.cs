using Godot;
using System;

namespace ODG.AI
{
	public class Arrive : SteeringBehavior
	{
		
		protected float maxSpeed, maxAcceleration, 
			slowdownRadius, targetRadius;
		
		public Arrive(
			Kinematic principal, 
			Kinematic target, 
			float maxSpeed, 
			float maxAcceleration,
			float slowdownRadius,
			float targetRadius
			)
		{
			Principal = new Kinematic(principal);
			Target = new Kinematic(target);
			this.maxSpeed = maxSpeed; 
			this.maxAcceleration = maxAcceleration;
			this.slowdownRadius = slowdownRadius;
			this.targetRadius = targetRadius;
		}
		
		public override SteeringOutput GetSteering()
		{
			SteeringOutput output = new SteeringOutput();
			Vector3 direction = Target.Position - Principal.Position;
			float distance = direction.Length();
			float distanceSquared = direction.LengthSquared();
			float targetSpeed = 0;

			if(distanceSquared < targetRadius * targetRadius) targetSpeed = 0;
			if(distanceSquared < slowdownRadius * slowdownRadius) targetSpeed = maxSpeed * distance / slowdownRadius;
			else targetSpeed = maxSpeed;

			Vector3 targetVelocity = direction.Normalized() * targetSpeed;

			output.linear = targetVelocity - Principal.Velocity;

			if(output.linear.Length() > maxAcceleration)
			{
				output.linear = output.linear.Normalized() * maxAcceleration;
			}
			return output;
		}
		
		public Kinematic Principal{ get; set; }
		
		public Kinematic Target{ get; set; }
	}
}

