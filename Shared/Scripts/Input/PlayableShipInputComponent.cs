using Godot;
using System;
using ODG.Input;

public class PlayableShipInputInterpreter : Node, IGameInputInterpreter
{
	PhysicsBody ship;
	GameInputPacket inputPacket = null;
	private bool isSelected = false;
	private bool packetUsed = false;
	private Vector3 target;
	private bool isTargetSet = false;
	
	public PlayableShipInputInterpreter() {}
	
	public PlayableShipInputInterpreter(PhysicsBody ship)
	{
		this.ship = ship;
		isSelected = false;
	}
	
	public override void _Ready()
	{
	}
	
	public override void _Process(float delta)
	{
		if(inputPacket is null) return;
		if(!packetUsed)
		{
			if(inputPacket.gameObjectId == (int)ship.GetInstanceId())
			{
				isSelected = !isSelected;
			}
			else if(inputPacket.gameObject.CollisionLayer != ship.CollisionLayer)
			{
				isSelected = false;
				isTargetSet = true;
				target = inputPacket.collisionPoint;
			}
			ship.GetChild<Spatial>(2).Visible = isSelected;
			packetUsed = true;
		}
	}
	
	public bool IsTargetSet()
	{
		return isTargetSet;
	}
	
	public Vector3 GetTarget()
	{
		isTargetSet = false;
		return target;
	}
	
	public bool IsSelected() { return isSelected; }
	
	public void ConnectToGameInputManager(GameInputManager gameInputManager)
	{
		gameInputManager.Connect("GameInputCaptured", this, nameof(OnGameInputCaptured));
	}
	
	public void OnGameInputCaptured(CustomObjectWrapper capturedInputPacket)
	{
		this.inputPacket = capturedInputPacket.GetValue<GameInputPacket>();
		packetUsed = false;
	}
}
