using Godot;
using System;

public static class Math
{
	public static float MapToRangePi(float angle)
	{
		var result = (angle + Mathf.Pi) % (2 * Mathf.Pi);
		if (result < 0)
	   		result += (2 * Mathf.Pi);
		result -= Mathf.Pi;
		return result;
	}
	
	public static float RandomBinomial()
	{
	  var rng = new RandomNumberGenerator();
	  float rand = rng.RandfRange(-1, 1);
	  rng.Randomize();
	  return rand - rng.RandfRange(-1, 1);
	}
}
