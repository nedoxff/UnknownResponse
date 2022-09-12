using Godot;
using static Godot.GD;
using System;

public partial class SceneLoader : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private ColorRect _rect;
	private Node _currentScene;
	private bool _busy;
	[Export] public float TransitionTime;
	public override void _Ready()
	{
		_rect = GetNode<ColorRect>("ColorRect");
	}

	public void Transition(PackedScene scene, bool animate = true)
	{
		if (_busy)
		{
			PushWarning("Cannot transition while another transition is already in-progress!");
			return;
		}
		if (animate)
		{
			_busy = true;
			_rect.MouseFilter = Control.MouseFilterEnum.Stop;
			var tween = CreateTween();
			tween.TweenProperty(_rect, "color:a", 1f, TransitionTime)
				.SetTrans(Tween.TransitionType.Cubic)
				.SetEase(Tween.EaseType.InOut);
			tween.Finished += () =>
			{
				ReplaceScene(scene);
				tween = CreateTween();
				tween.TweenProperty(_rect, "color:a", 0f, TransitionTime)
					.SetTrans(Tween.TransitionType.Cubic)
					.SetEase(Tween.EaseType.InOut);
				tween.Finished += () =>
				{
					_rect.MouseFilter = Control.MouseFilterEnum.Ignore;
					_busy = false;
					Print($"Transitioned to {_currentScene.Name}");
				};
			};
		}
		else ReplaceScene(scene);
	}

	private void ReplaceScene(PackedScene scene)
	{
		var instance = scene.Instantiate();
		_currentScene?.QueueFree();
		_currentScene = instance;
		GetParent().AddChild(_currentScene);
		GetParent().MoveChild(_currentScene, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
