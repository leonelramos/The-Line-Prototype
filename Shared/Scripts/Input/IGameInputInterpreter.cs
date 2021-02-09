namespace ODG.Input
{
	interface IGameInputInterpreter
	{
		void ConnectToGameInputManager(GameInputManager gameInputManager);
		void OnGameInputCaptured(CustomObjectWrapper gameInputPacket);
	}
}
