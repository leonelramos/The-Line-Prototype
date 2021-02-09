using Godot;
using System;

namespace ODG.Input
{	
	public class GameInputManager : Node
	{
		private Camera camera;
		private World world;
		
		public GameInputManager() {}

		public GameInputManager(Camera camera, World world)
		{
			this.camera = camera;
			this.world = world;
		}

		[Signal]
		public delegate void GameInputCaptured(CustomObjectWrapper inputPacket);

		public override void _UnhandledInput(InputEvent inputEvent)
		{
			if(inputEvent is InputEventScreenTouch && !inputEvent.IsEcho() && !inputEvent.IsPressed())
			{
				GD.Print("Touch Input Captured...");
				var touch = ((InputEventScreenTouch)inputEvent);
				GameInputPacket inputPacket = new GameInputPacket();
				var worldQuery = Utils3D.WorldQueryFromScreenPosition(touch.Position, world, camera);
				if(worldQuery.Count > 0)
				{
					GD.Print(worldQuery);
					inputPacket.gameObject = (PhysicsBody)worldQuery["collider"];
					inputPacket.gameObjectId = (int)worldQuery["collider_id"];
					inputPacket.collisionPoint = (Vector3)worldQuery["position"];
					inputPacket.collisionNormal = (Vector3)worldQuery["normal"];
				}
				GD.Print("Game Input Packet Created And Emitted...");
				EmitSignal(nameof(GameInputCaptured), new CustomObjectWrapper(inputPacket));
			}
		}
	}
	
	public class GameInputPacket
	{
		public PhysicsBody gameObject = null;
		public int gameObjectId = 0;
		public Vector3 collisionPoint = Vector3.Inf;
		public Vector3 collisionNormal = Vector3.Zero;
		public GameInputPacket() {}
	}
}
