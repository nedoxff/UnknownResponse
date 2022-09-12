using Godot;
using System;

public partial class Main : Node2D
{
	private SceneLoader _loader;

	[Export] public PackedScene StartScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_loader = GetNode<SceneLoader>("SceneLoader");
		_loader.Transition(StartScene, false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
