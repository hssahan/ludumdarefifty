using Godot;
using System;
using System.Collections.Generic;

public class Plant : Area2D
{
    [Export] public NodePath TimerNodePath;
    [Export] public List<NodePath> SpriteNodePaths;
    // [Export] public NodePath DamageAreaNodePath;
    [Export] public NodePath HpLabelNodePath;
    private List<Sprite> _sprites = new List<Sprite>();

    private Level _currentLevel = Level.One;
    private Sprite _currentSprite;
    private Timer _timer;
    // private Area2D _damageArea;
    private Label _hpLabel;
    private float _health = 100;

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


        Connect("area_entered", this, nameof(AreaEntered));
        Connect("area_exited", this, nameof(AreaExited));
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

    internal void TakeDamage(float damage)
    {
        _health -= damage;
        _hpLabel.Text = $"{(int)_health}";
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