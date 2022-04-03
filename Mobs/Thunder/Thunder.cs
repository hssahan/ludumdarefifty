using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class Thunder : KinematicBody2D
{
    [Export] public NodePath PlayerNodePath;
    [Export] public NodePath TimerNodePath;
    [Export] public NodePath AnimationPlayerNodePath;
    [Export] public NodePath FireParentNodePath;
    private readonly PackedScene _smallFireScene = (PackedScene)ResourceLoader.Load("res://Mobs/SmallFire/SmallFire.tscn");
    private Timer _timer;
    private AnimationPlayer _animationPlayer;
    private Random _random;
    private Vector2 _nextPosition = Vector2.Zero;
    private Node2D _fireParentNode;
    private Player _player;
    private int difficultyCounter = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _timer = GetNode<Timer>(TimerNodePath);
        _animationPlayer = GetNode<AnimationPlayer>(AnimationPlayerNodePath);
        _fireParentNode = GetNode<Node2D>(FireParentNodePath);
        _player = GetNode<Player>(PlayerNodePath);

        _random = new Random();
        _timer.Connect("timeout", this, nameof(HitThunder));

        FinishThunderbolt();
    }

    private void FinishThunderbolt()
    {
        _timer.Start(_random.Next(10 / difficultyCounter, 40 / difficultyCounter));
        _nextPosition = new Vector2(_random.Next(70, 150), 0);
        difficultyCounter += 1;
    }

    private void SpawnFire()
    {
        var spaceState = GetWorld2d().DirectSpaceState;
        var result = spaceState.IntersectRay(GlobalPosition, GlobalPosition + new Vector2(0, 500), new Godot.Collections.Array() { this, _player },
        collisionLayer: 1);
        foreach (var k in result.Keys)
        {
            if (k is string key && key == "position")
            {
                var v = result[k];
                var smallFire = _smallFireScene.Instance<SmallFire>();
                _fireParentNode.AddChild(smallFire);
                smallFire.GlobalPosition = (Vector2)v;
            }
        }
    }

    private void HitThunder()
    {
        GlobalPosition = _nextPosition;
        _animationPlayer.Play("hit");
    }
}
