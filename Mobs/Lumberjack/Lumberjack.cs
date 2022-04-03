using Godot;
using System;

public class Lumberjack : KinematicBody2D
{
    [Export] public NodePath VicinityNodePath;
    [Export] public NodePath DamageTakingAreaNodePath;
    [Export] public NodePath AnimationPlayerNodePath;
    private string _lastDirection;
    private AnimationPlayer _animationPlayer;
    private Vector2 _movement = Vector2.Zero;
    private Area2D _vicinity;
    private Area2D _damageTakingArea;
    private Plant _plant;
    private bool _isHitting = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _lastDirection = Direction.Right;
        _animationPlayer = GetNode<AnimationPlayer>(AnimationPlayerNodePath);
        _vicinity = GetNode<Area2D>(VicinityNodePath);
        _damageTakingArea = GetNode<Area2D>(DamageTakingAreaNodePath);

        _vicinity.Connect("area_entered", this, nameof(AreaEntered));
        _damageTakingArea.Connect("body_entered", this, nameof(BodyEntered));
    }

    private void BodyEntered(Node2D node)
    {
        if(node is Player player)
        {
            player.DamageDealt();
            QueueFree();
        }
    }

    private void AreaEntered(Area2D area)
    {
        if (area is Plant plant)
        {
            _plant = plant;
            _animationPlayer.Play($"hit-{_lastDirection}");
            _isHitting = true;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!_isHitting)
        {
            _animationPlayer.Play("run-right");
            _movement.x = 15;
            _movement.y += 700 * delta;
            _movement = MoveAndSlide(_movement, new Vector2(0, -1));
        }
    }
    private void ApplyDamage()
    {
        _plant.TakeDamage(5f);
    }
}
