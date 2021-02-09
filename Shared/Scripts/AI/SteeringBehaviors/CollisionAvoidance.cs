using Godot;
using System;
using System.Collections.Generic;

namespace ODG.AI
{
	public class CollisionAvoidance : SteeringBehavior
	{
		public Kinematic Principal{ get; set; }
		public List<Kinematic> Targets{ get; set; }
		public float Radius{ get; set; }
		public float MaxAcceleration{ get; set; }
		
		public CollisionAvoidance(Kinematic principal, List<Kinematic> targets, float radius, float maxAcceleration)
		{
			Principal = principal;
			Targets = targets;
			Radius = radius;
			MaxAcceleration = maxAcceleration;
		}

		public override SteeringOutput GetSteering()
		{
			SteeringOutput output = new SteeringOutput();
			var shortestTime = float.MaxValue;

			Kinematic closestTarget = null;
			float closestMinSeperation = float.MaxValue;
			float closestDistance = float.MaxValue;
			Vector3 closestRelativePos = Vector3.Zero;
			Vector3 closestRelativeVel = Vector3.Zero;

			foreach(Kinematic target in Targets)
			{
				var relativePos = target.Position - Principal.Position;
				var relativeVel = target.Velocity - Principal.Velocity;
				var relativeSpeed = relativeVel.Length();
				var timeToCollision = relativePos.Dot(relativeVel)/relativeVel.Dot(relativeVel);
				
				

				var distance = relativePos.Length();
				var minSeperation = distance - relativeSpeed * timeToCollision;
				GD.Print("sep: " + minSeperation);
				if (minSeperation > 2 * Radius)
					continue;
				
				if(timeToCollision > 0 && timeToCollision < shortestTime)
				{
					GD.Print("rel_pos: " + relativePos);
					GD.Print("rel_vel: " + relativeVel);
					GD.Print("time: " + timeToCollision);
					GD.Print("dist: " + distance);
					shortestTime = timeToCollision;
					closestTarget = target;
					closestMinSeperation = minSeperation;
					closestDistance = distance;
					closestRelativePos = relativePos;
					closestRelativeVel = relativeVel;
				}
			}
			if(closestTarget is null) return output;
			GD.Print("gonna collide");
			if(closestMinSeperation <= 0 || closestDistance <= 2 * Radius)
				closestRelativePos = closestTarget.Position - Principal.Position;
			else
				closestRelativePos += closestRelativeVel * shortestTime;

			closestRelativePos = closestRelativePos.Normalized();
			output.linear = closestRelativePos * MaxAcceleration;
			return output;
		}
	}
}

