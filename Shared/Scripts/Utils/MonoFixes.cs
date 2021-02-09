using Godot;
using System;

public static class MonoFixes
{
	
}

public class CustomObjectWrapper : Godot.Object
{
	private readonly object value;

	public CustomObjectWrapper(object value)
	{
		this.value = value;
	}

	public T GetValue<T>()
	{
		return (T)this.value;
	}
}
