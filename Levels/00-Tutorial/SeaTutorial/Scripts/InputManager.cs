using Godot;
using System;
using ODG.Input;

public class InputManager : Node
{
	private GameInputManager game_input_manager;
	
	public override void _Ready()
	{
		game_input_manager = new GameInputManager(GetNode<Camera>("World/Camera"), GetNode<Spatial>("World").GetWorld());
		AddChild(game_input_manager);
		ConnectToGameInputInterpreters(this);
	}
	
	private void ConnectToGameInputInterpreters(Node scene_tree)
	{
		if(scene_tree is IGameInputInterpreter)
			(scene_tree as IGameInputInterpreter).ConnectToGameInputManager(game_input_manager);
		
		int sub_tree_count = scene_tree.GetChildCount();
		
		for(int i = 0; i < sub_tree_count; i++)
			ConnectToGameInputInterpreters(scene_tree.GetChild(i));
	}
}
