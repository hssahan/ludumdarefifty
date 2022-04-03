using Godot;
using System;

public class Menu : Node2D
{
    [Export] public NodePath MainGameNodePath;
    [Export] public NodePath StartButtonNodePath;
    [Export] public NodePath RestartButtonNodePath;
    [Export] public NodePath StartMenuNodePath;
    [Export] public NodePath GameOverMenuNodePath;
    [Export] public NodePath GameOverLabelNodePath;
    private Button _startButton;
    private Button _restartButton;
    private Node2D _mainGame;
    private Node2D _startMenu;
    private Node2D _gameOverMenu;
    private Label _gameOverLabel;
    private readonly PackedScene _gameScene = (PackedScene)ResourceLoader.Load("res://MainGameScene.tscn");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _mainGame = GetNode<Node2D>(MainGameNodePath);
        _startButton = GetNode<Button>(StartButtonNodePath);
        _startMenu = GetNode<Node2D>(StartMenuNodePath);
        _restartButton = GetNode<Button>(RestartButtonNodePath);
        _gameOverMenu = GetNode<Node2D>(GameOverMenuNodePath);
        _gameOverLabel = GetNode<Label>(GameOverLabelNodePath);

        _startButton.Connect("pressed", this, nameof(StartGame));
        _restartButton.Connect("pressed", this, nameof(RestartGame));
        GetTree().Paused = true;
        _startMenu.Visible = true;
        _gameOverMenu.Visible = false;
        
    }

    private void RestartGame()
    {
        var newGame = _gameScene.Instance<Node2D>();
        _mainGame.QueueFree();
        AddChild(newGame);
        _mainGame = newGame;
        StartGame();
    }

    private void StartGame()
    {
        GetTree().Paused = false;
        _startMenu.Visible = false;
        _gameOverMenu.Visible = false;
    }

    public void GameOver(string timeSurvived)
    {
        GetTree().Paused = true;
        _startMenu.Visible = false;
        _gameOverLabel.Text = $"Time plant kept alive:\n{timeSurvived}";
        _gameOverMenu.Visible = true;
    }
}
