using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Plant : Area2D
{
    [Export] public NodePath MenuNodePath;
    [Export] public NodePath TimerNodePath;
    [Export] public List<NodePath> SpriteNodePaths;
    [Export] public NodePath TimeSurvivedLabelNodePath;
    [Export] public NodePath HpLabelNodePath;
    private List<Sprite> _sprites = new List<Sprite>();

    private Level _currentLevel = Level.One;
    private Sprite _currentSprite;
    private Timer _timer;
    // private Area2D _damageArea;
    private Label _hpLabel;
    private float _health = 100;
    private Menu _menu;
    public Stopwatch Stopwatch = new Stopwatch();
    private Label _timeSurvivedLabel;

    public string ElapsedTime { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (var spriteNodePath in SpriteNodePaths)
        {
            var sprite = GetNode<Sprite>(spriteNodePath);
            sprite.Visible = false;
            _sprites.Add(sprite);
        }

        _currentSprite = _sprites[(int)_currentLevel];
        _currentSprite.Visible = true;

        _timer = GetNode<Timer>(TimerNodePath);
        _timer.Connect("timeout", this, nameof(LevelUp));
        _timer.Start(5 * (int)_currentLevel+1);

        // _damageArea = GetNode<Area2D>(DamageAreaNodePath);
        _hpLabel = GetNode<Label>(HpLabelNodePath);
        _menu = GetNode<Menu>(MenuNodePath);
        _timeSurvivedLabel = GetNode<Label>(TimeSurvivedLabelNodePath);


        Connect("area_entered", this, nameof(AreaEntered));
        Connect("area_exited", this, nameof(AreaExited));
    }

    public override void _Process(float delta)
    {
        if(!Stopwatch.IsRunning)
        {
            Stopwatch.Start();
        }
        _timeSurvivedLabel.Text = FormatTimeSurvived();
    }

    private string FormatTimeSurvived()
    {
        return String.Format("{1:00}:{2:00}.{3:00}",
            Stopwatch.Elapsed.Hours, Stopwatch.Elapsed.Minutes, Stopwatch.Elapsed.Seconds,
            Stopwatch.Elapsed.Milliseconds / 10);
    }

    private void AreaExited(Area2D area)
    {
        if (area is SmallFire fire)
        {
            fire.Plant = null;
        }
    }

    private void AreaEntered(Area2D area)
    {
        if (area is SmallFire fire)
        {
            fire.Plant = this;
        }
    }

    private void LevelUp()
    {
        if (!Enum.IsDefined(typeof(Level), _currentLevel + 1))
        {
            return;
        }

        _currentSprite.Visible = false;
        _currentLevel = (Level)_currentLevel + 1;
        _currentSprite = _sprites[(int)_currentLevel];
        _currentSprite.Visible = true;
        _timer.Start(5 * (int)_currentLevel+1);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _hpLabel.Text = $"{(int)_health}";
        if(_health <= 0)
        {
            Stopwatch.Stop();
            _menu.GameOver(FormatTimeSurvived());
        }
    }
}

public enum Level
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven
}