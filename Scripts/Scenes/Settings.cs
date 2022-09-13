using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public partial class Settings : Node2D
{
	private SceneLoader _loader;
	private int _current;
	private List<ColorRect> _options;
	private List<VBoxContainer> _containers;
	[Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;
	[Export] public float SwitchDuration;
	public override void _Ready()
	{
		_options = GetNode("HBoxContainer").GetChildren().OfType<ColorRect>().ToList();
		_containers = GetChildren().OfType<VBoxContainer>().ToList();
		_loader = GetNode<SceneLoader>("/root/Main/SceneLoader");
	}
	
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("ui_cancel"))
			_loader.Transition(ResourceLoader.Load<PackedScene>(MainMenuScene));
		if(Input.IsActionJustPressed("ui_right") && _current + 1 <= _options.Count - 1)
			Select(_current + 1);
		else if(Input.IsActionJustPressed("ui_left") && _current - 1 >= 0)
			Select(_current - 1);
	}

	private void Select(int index)
	{
		var tween = CreateTween().SetParallel();
		tween.TweenProperty(_options[_current], "color", new Color(0, 0, 0), SwitchDuration);
		tween.TweenProperty(_containers[_current], "modulate:a", 0, SwitchDuration);
		_current = index;
		tween.TweenProperty(_options[index], "color", new Color(20 / 255f, 20 / 255f, 20 / 255f), SwitchDuration);
		tween.TweenProperty(_containers[index], "modulate:a", 1, SwitchDuration);
	}
}
