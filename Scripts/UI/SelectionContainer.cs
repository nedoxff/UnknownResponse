using System.Collections.Generic;
using Godot;
using Godot.Collections;
using static Godot.GD;

namespace UnknownResponse.Scripts.UI;

public partial class SelectionContainer : VBoxContainer
{
	[Export] public Array<string> Options;
	[Export] public LabelSettings LabelSettings;
	[Export] public string Cursor;
	[Export] public float ShowDuration;
	[Export] public float MoveDuration;
	[Signal] public delegate void SelectedEventHandler(string name);

	private int _current;
	private MarginContainer _marginContainer;
	private VBoxContainer _vboxContainer;
	private Label _cursor;
	private Tween _tween;
	private readonly List<Label> _labelOptions = new();
	public override void _Ready()
	{
		_marginContainer = GetNode<MarginContainer>("HBoxContainer/MarginContainer");
		_vboxContainer = GetNode<VBoxContainer>("HBoxContainer/VBoxContainer");
		_cursor = new Label();
		_cursor.LabelSettings = LabelSettings;
		_cursor.Text = Cursor;
		_cursor.Name = "Cursor";
		_marginContainer.AddChild(_cursor);
		foreach (var option in Options)
		{
			var label = new Label();
			label.LabelSettings = LabelSettings;
			label.Name = option;
			label.Text = option;
			_vboxContainer.AddChild(label);
			_labelOptions.Add(label);
		}
		Print($"Created SelectionContainer with {Options.Count} options");
	}

	public void AnimatedShow()
	{
		Show();
		_tween = CreateTween();
		_tween.SetParallel();
		foreach (var option in _labelOptions)
		{
			option.VisibleCharacters = 0;
			_tween.TweenProperty(option, "visible_characters", option.Text.Length, ShowDuration)
				.SetEase(Tween.EaseType.InOut)
				.SetTrans(Tween.TransitionType.Cubic);
		}

		_cursor.Modulate = Color.Color8(255, 255, 255, 0);
		_tween.TweenProperty(_cursor, "modulate:a", 1, ShowDuration)
			.SetEase(Tween.EaseType.InOut)
			.SetTrans(Tween.TransitionType.Cubic);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_down"))
			MoveTo(_current + 1 > Options.Count - 1 ? 0: _current + 1);
		else if(Input.IsActionJustPressed("ui_up"))
			MoveTo(_current - 1 < 0 ? Options.Count - 1: _current - 1);
		else if (Input.IsActionJustPressed("ui_accept"))
			EmitSignal("Selected", Options[_current]);
	}

	private void MoveTo(int index)
	{
		_current = index;
		_tween = CreateTween();
		_tween.TweenProperty(_marginContainer, "offset_top", _labelOptions[index].OffsetTop, MoveDuration)
			.SetEase(Tween.EaseType.InOut)
			.SetTrans(Tween.TransitionType.Circ);
	}
}
