using Godot;
using UnknownResponse.Scripts.UI;

public partial class MainMenu : Node2D
{
	private Label _title;
	private double _previousDelta;
	private bool _blink;
	private SelectionContainer _selectionContainer;
	private SceneLoader _loader;

	[Export] public float ShowDuration = 1.0f;
	[Export] public double BlinkDelay = 1.0;
	[Export] public string Cursor = "|";
	[Export] public PackedScene SettingsScene;
	
	public override void _Ready()
	{
		_title = GetNode<Label>("Title");
		_loader = GetNode<SceneLoader>("/root/Main/SceneLoader");
		_selectionContainer = GetNode<SelectionContainer>("SelectionContainer");
		_selectionContainer.Selected += t =>
		{
			switch (t)
			{
				case "Exit":
					GetTree().Quit();
					break;
				case "Settings":
					_loader.Transition(SettingsScene);
					break;
			}
		};
		_title.VisibleCharacters = 0;
		var tween = CreateTween();
		tween.TweenProperty(_title, "visible_characters", _title.Text.Length + 1, ShowDuration)
			.SetTrans(Tween.TransitionType.Cubic)
			.SetEase(Tween.EaseType.InOut);
		tween.Finished += () =>
		{
			_blink = true;
			_selectionContainer.AnimatedShow();
		};
	}
	
	public override void _Process(double delta)
	{
		if (!_blink) return;
		_previousDelta += delta;
		if (!(_previousDelta >= BlinkDelay)) return;
		_previousDelta = 0;
		_title.Text = _title.Text.EndsWith(Cursor) ? _title.Text[..^1] : _title.Text + Cursor;
	}
}
