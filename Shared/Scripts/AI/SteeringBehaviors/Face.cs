using Godot;
using System;

namespace ODG.AI
{
	public class Face : Align
	{
		public Face(
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
			Vector3 direction = Target.Position - Principal.Position;
			Target.Orientation = Math3D.VectorOrientation(direction);
			return base.GetSteering();
		}
	}
}
