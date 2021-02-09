using Godot;
using System;

namespace ODG.AI
{
	public class Seek : SteeringBehavior
	{
		
		
		public Seek(Kinematic principal, Kinematic target)
		{
			Principal = new Kinematic(principal);
			Target = new Kinematic(target);
		}
		
		public override SteeringOutput GetSteering()
		{
			SteeringOutput output = new SteeringOutput();
			Vector3 targetVelocity = Target.Position - Principal.Position;
	
			output.linear = targetVelocity;
			output.angular = 0;
			return output;
		}
		
		public Kinematic Principal{ get; set; }
		
		public Kinematic Target{ get; set; }
	}
}

