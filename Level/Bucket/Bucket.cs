using Godot;
using System;

public class Bucket : Node2D
{
    [Export] public NodePath VicinityNodePath;
    [Export] public NodePath GuideLabelNodePath;
    [Export] public NodePath WaterDropParticlesNodePath;
    [Export] public NodePath WaterSplashSoundNodePath;
    private AudioStreamPlayer _waterSplashSound;
    private CPUParticles2D _waterDropParticles;
    private Area2D _vicinity;
    private bool _isPlayerInVicinity = false;
    private Player _player;
    private Label _guideLabel;
    private bool _isPickedUp = false;
    public bool HasWater = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _waterDropParticles = GetNode<CPUParticles2D>(WaterDropParticlesNodePath);
        _guideLabel = GetNode<Label>(GuideLabelNodePath);
        _vicinity = GetNode<Area2D>(VicinityNodePath);
        _waterSplashSound = GetNode<AudioStreamPlayer>(WaterSplashSoundNodePath);

        _vicinity.Connect("body_entered", this, nameof(BodyEntered));
        _vicinity.Connect("body_exited", this, nameof(BodyExited));
        _waterDropParticles.Emitting = false;
    }
    public override void _PhysicsProcess(float delta)
    {
        if (_isPickedUp && _player != null)
        {
            GlobalPosition = _player.GlobalPosition - new Vector2(-10, -3);
        }
        if(_isPlayerInVicinity && _player != null && !_isPickedUp)
        {
            if (Input.IsActionPressed("action"))
            {
                _player.PicksUpBucket(this);
                _isPickedUp = true;
                _guideLabel.Visible = false;
            }
        }

    }

    public void FillWater()
    {
        _waterDropParticles.Emitting = true;
        HasWater = true;
        _waterSplashSound.Play();
        
    }
    public void UseWater()
    {
        _waterDropParticles.Emitting = false;
        HasWater = false;
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
