using Godot;
using System;

namespace ODG.AI
{
	public class SteeringBuilder
	{
		Kinematic principal, target;
		// Linear forces
		float maxSpeed;
		float maxAcceleration;
		float linearSlowdownRadius;
		float linearTargetRadius;

		// Angular forces
		float maxRotation;
		float maxAngularAcceleration;
		float angularSlowdownRadius;
		float angularTargetRadius;

		// Wander only
		float wanderOffset;
		float wanderRadius;
		float wanderRate;

		public enum Behavior
		{
			Align,
			Arrive,
			Face,
			FaceForward,
			Seek,
			Wander
		}
		
		public SteeringBuilder(Behavior behavior)
		{

		}
	}
}
