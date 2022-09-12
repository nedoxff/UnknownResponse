using Godot;
using System;

public partial class Main : Node2D
{
	private SceneLoader _loader;
	[Export] public PackedScene StartScene;
	
	public override void _Ready()
	{
		_loader = GetNode<SceneLoader>("SceneLoader");
		_loader.Transition(StartScene, false);
	}
	
	public override void _Process(double delta)
	{
	}
}
