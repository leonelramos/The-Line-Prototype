using Godot;
using System;

namespace ODG.AI
{
	public class FaceForward : Align
	{
		public FaceForward(
			Kinematic principal, 
			Kinematic target, 
			float maxRotation, 
			float maxAngularAcceleration,
			float slowdownRadius,
			float targetRadius
			) : base(
			principal, 
			target, 
			maxRotation, 
			maxAngularAcceleration,
			slowdownRadius,
			targetRadius
			)
		{}
		public override SteeringOutput GetSteering()
		{
			Vector3 direction = Principal.Velocity;
			
			if(direction.LengthSquared() < Mathf.Epsilon)
				Target.Orientation = Principal.Orientation;
			else 
				Target.Orientation = Math3D.VectorOrientation(direction);
				
			return base.GetSteering();
		}
	}
}
