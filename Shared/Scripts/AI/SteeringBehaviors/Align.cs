using Godot;
using System;

namespace ODG.AI
{
	public class Align : SteeringBehavior
	{
		
		protected float maxRotation, maxAngularAcceleration, 
			slowdownRadius, targetRadius;

		public Align(
			Kinematic principal, 
			Kinematic target, 
			float maxRotation, 
			float maxAngularAcceleration,
			float slowdownRadius,
			float targetRadius
			)
		{
			Principal = new Kinematic(principal);
			Target = new Kinematic(target);
			this.maxRotation = maxRotation; 
			this.maxAngularAcceleration = maxAngularAcceleration;
			this.slowdownRadius = slowdownRadius;
			this.targetRadius = targetRadius;
		}
		
		public override SteeringOutput GetSteering()
		{
			SteeringOutput output = new SteeringOutput();
			float targetRotation = 0;
			float seperationAngle = Target.Orientation - Principal.Orientation;
			seperationAngle = Math.MapToRangePi(seperationAngle);
			float angularDistance = Mathf.Abs(seperationAngle);
			if(angularDistance < targetRadius) targetRotation = 0;
			if(angularDistance > slowdownRadius) targetRotation = maxRotation;
			else targetRotation = maxRotation * angularDistance / slowdownRadius;
			
			if(angularDistance != 0)
				targetRotation *= (seperationAngle / angularDistance);
			
			output.angular = targetRotation - Principal.Rotation;

			float angularAcceleration = Mathf.Abs(output.angular);
			if(angularAcceleration > maxAngularAcceleration)
			{
				output.angular *= (1F / angularAcceleration) * maxAngularAcceleration; 
			}
			output.linear = Vector3.Zero;
			return output;
		}
		
		public Kinematic Principal{ get; set; }
		
		public Kinematic Target{ get; set; }
	}
}

