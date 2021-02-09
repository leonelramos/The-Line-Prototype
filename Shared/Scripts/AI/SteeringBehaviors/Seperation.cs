using Godot;
using System;
using System.Collections.Generic;

namespace ODG.AI
{
	public class Seperation : SteeringBehavior
	{
		float maxAcceleration, decay, threshold;

		public Seperation(Kinematic principal, float maxAcceleration, float decay, float threshold,  params Kinematic[] targets)
		{
			Targets = new List<Kinematic>();
			Principal = new Kinematic(principal);
			this.maxAcceleration = maxAcceleration;
			this.decay = decay;
			this.threshold = threshold;
			foreach(Kinematic target in targets) Targets.Add(target);
		}
		
		public Seperation(Kinematic principal, float maxAcceleration, float decay, float threshold,  List<Kinematic> targets)
		{
			Principal = new Kinematic(principal);
			this.maxAcceleration = maxAcceleration;
			this.decay = decay;
			this.threshold = threshold;
			Targets = targets;
		}

		public override SteeringOutput GetSteering()
		{
			SteeringOutput output = new SteeringOutput();
			int targetCount = 0;
			foreach(Kinematic target in Targets)
			{
				Vector3 direction = Principal.Position - target.Position;
				float distance = direction.Length();

				if(distance < threshold)
				{
					float strength = Mathf.Min(decay / distance * distance, maxAcceleration);
					output.linear += strength * direction.Normalized();
				}
				targetCount++;
			}
			if(targetCount > 0) output.linear *= (1F/targetCount);
			return output;
		}
		
		public List<Kinematic> Targets{ get; set; }

		public Kinematic Principal{ get; set; }
	}
}
