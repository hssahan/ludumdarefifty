using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public NodePath AnimationPlayerNodePath;
    [Export] public NodePath HealthPointsLabelNodePath;
    private AnimationPlayer _animationPlayer;
    private Label _healthPointsLabel;
    private string _lastDirection;
    private bool _isJumping = false;
    private Vector2 _movement = Vector2.Zero;
    private int _movementSpeed = 40;

    private float HealthPoints = 100;
    private Bucket _bucket;

    public bool HasWater => _bucket?.HasWater ?? false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _lastDirection = Direction.Left;
        _animationPlayer = GetNode<AnimationPlayer>(AnimationPlayerNodePath);
        _healthPointsLabel = GetNode<Label>(HealthPointsLabelNodePath);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_isJumping && IsOnFloor())
        {
            _isJumping = false;
        }
        _movement.x = 0;
        var animationToPlay = $"idle-{_lastDirection}";

        if (Input.IsActionPressed("run-right"))
        {
            _movement.x += _movementSpeed;
            animationToPlay = "run-right";
            _lastDirection = Direction.Right;
        }

        if (Input.IsActionPressed("run-left"))
        {
            _movement.x -= _movementSpeed;
            animationToPlay = "run-left";
            _lastDirection = Direction.Left;
        }
        if (Input.IsActionJustPressed("jump") && !_isJumping)
        {
            _movement.y = -200;
            _isJumping = true;
        }
        if (_movement.x == 0)
        {
            animationToPlay=$"idle-{_lastDirection}";
        }
        if(_isJumping)
        {
            animationToPlay = $"jump-{_lastDirection}";
        }
        _animationPlayer.Play(animationToPlay);

        _movement.y += 700 * delta;


        _movement = MoveAndSlide(_movement, new Vector2(0, -1));
    }

    internal void PicksUpBucket(Bucket bucket)
    {
        _bucket = bucket;
    }

    public void DamageDealt()
    {
        _movement.y -= 150;
    }

    public void UseWater()
    {
        _bucket.UseWater();
    }

    public void TakesWater()
    {
        _bucket.FillWater();
    }

    public void TakeDamage(float damage)
    {
        HealthPoints -= damage;
        _healthPointsLabel.Text = $"{(int)HealthPoints}";
    }
}

public class Direction
{
    public const string Left = "left";
    public const string Right = "right";
}