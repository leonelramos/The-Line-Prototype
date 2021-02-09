using Godot;
using System;

namespace ODG.AI
{
	public class Kinematic
	{
		private float orientation;
		private Vector3 maxVelocity;
		private float maxRotation;
		
		public Kinematic(Vector3 position, float orientation)
		{
			Position = position;
			Orientation = orientation;
			Velocity = Vector3.Zero;
			Rotation = 0;
		}

		public Kinematic(Kinematic other)
		{
			Position = other.Position;
			Orientation = other.Orientation;
			Velocity = other.Velocity;
			Rotation = other.Rotation;
		}
		
		public float Orientation
		{
			get { return orientation; }
			set { orientation = Math.MapToRangePi(value); }
		}
		public float Rotation { get; set; }
		public Vector3 Position { get; set; }
		public Vector3 Velocity { get; set; }
		
		public void MakeStationary()
		{
			Velocity = Vector3.Zero;
			Rotation = 0;
		}
	}
	
	public class SteeringOutput
	{
		public SteeringOutput()
		{
			linear = Vector3.Zero;
			angular = 0;
		}
		public Vector3 linear;
		public float angular;

		public static SteeringOutput operator+(SteeringOutput a, SteeringOutput b)
		{
			SteeringOutput sum = new SteeringOutput();
			sum.linear = a.linear + b.linear;
			sum.angular = a.angular + b.angular;
			return sum;
		}
		
		public static SteeringOutput operator*(float a, SteeringOutput b)
		{
			return b * a;
		}
		
		public static SteeringOutput operator*(SteeringOutput b, float a)
		{
			SteeringOutput product = new SteeringOutput();
			product.linear = a * b.linear;
			product.angular = a * b.angular;
			return product;
		}
	}
	
	public abstract class SteeringBehavior
	{
		public abstract SteeringOutput GetSteering();
	}
}
