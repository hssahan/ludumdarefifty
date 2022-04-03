using Godot;
using System;

public class WaterRefillStation : Node2D
{
    [Export] public NodePath VicinityNodePath;
    private Area2D _vicinity;
    private bool _isPlayerInVicinity = false;
    private Player _player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
        _vicinity = GetNode<Area2D>(VicinityNodePath);
        _vicinity.Connect("body_entered", this, nameof(BodyEntered));
        _vicinity.Connect("body_exited", this, nameof(BodyExited));
    }

    public override void _PhysicsProcess(float delta)
    {
        if(_isPlayerInVicinity && _player != null)
        {
            if (Input.IsActionPressed("action"))
            {
                _player.TakesWater();
            }
        }
    }

    private void BodyEntered(Node2D node)
    {
        if (node is Player player)
        {
            _isPlayerInVicinity = true;
            _player = player;
        }
    }

    private void BodyExited(Node2D node)
    {
        if (node is Player)
        {
            _isPlayerInVicinity = false;
            _player = null;
        }
    }
}
