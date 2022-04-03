using Godot;
using System;

public class SmallFire : Area2D
{
    [Export] public NodePath DamageAreaNodePath;
    [Export] public NodePath ExtinguishFireSoundNodePath;
    private AudioStreamPlayer _extinguishFireSound;
    private const int FullDamage = 1;
    private Random _random;
    private bool _isPlayerInVicinity;
    private Player _player;
    private bool _isHit = false;
    public Plant Plant;
    private bool _isPlayerInDamageArea = false;
    private float _fireSpreadMultiplier;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _random = new Random();
        var timerUntilSpread = new Timer();
        AddChild(timerUntilSpread);
        timerUntilSpread.Start(_random.Next(5, 10));
        // timerUntilSpread.Connect("timeout", this, nameof(SpreadFire));
        var damageArea = GetNode<Area2D>(DamageAreaNodePath);
        _extinguishFireSound = GetNode<AudioStreamPlayer>(ExtinguishFireSoundNodePath);

        damageArea.Connect("body_entered", this, nameof(DamageAreaEntered));
        damageArea.Connect("body_exited", this, nameof(DamageAreaExited));

        Connect("body_entered", this, nameof(BodyEntered));
        Connect("body_exited", this, nameof(BodyExited));
        var spawnScale = _random.Next(1, 3);
        Scale = Scale * spawnScale;

        _fireSpreadMultiplier = _random.Next(1, 3);
    }

    private void DamageAreaExited(Node2D node)
    {
        if (node is Player player)
        {
            _isPlayerInDamageArea = false;
        }
    }

    private void DamageAreaEntered(Node2D node)
    {
        if (node is Player player)
        {
            _isPlayerInDamageArea = true;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        var scaleIncrease = 0.001f * _fireSpreadMultiplier;
        Scale = Scale + new Vector2(scaleIncrease, scaleIncrease);

        if (_isPlayerInVicinity && _player != null)
        {
            if (Input.IsActionJustPressed("action") && _player.HasWater)
            {
                _extinguishFireSound.Play();
                _player.UseWater();
                if (!_isHit)
                {
                    Scale = Scale / 2f;
                    _isHit = true;
                }
                else
                {
                    QueueFree();
                }
                // _player.TakesWater();
                // _isPickedUp = true;
            }
        }
        if (Plant != null)
        {
            Plant.TakeDamage(FullDamage * (Scale.x / 20));
        }
        if (_isPlayerInDamageArea)
        {
            _player.TakeDamage(0.2f);
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




    private void SpreadFire()
    {
        // var spaceState = GetWorld2d().DirectSpaceState;
        // var checkRightResult = spaceState.IntersectRay(GlobalPosition, GlobalPosition + new Vector2(20, 0), new Godot.Collections.Array() { this });
        // var checkLeftResult = spaceState.IntersectRay(GlobalPosition, GlobalPosition + new Vector2(-20, 0), new Godot.Collections.Array() { this });

        // foreach(var k in checkRightResult.Keys)
        // {
        //     if(k is string key && key == "collider")
        //     {
        //         var collidedWith = checkRightResult[key];
        //         if(collidedWith is SmallFire)
        //         {
        //             break;
        //         }
        //     }
        //     if(k is string key && key == "position")
        //     {
        //         var v = result[k];
        //         var smallFire = _smallFireScene.Instance<SmallFire>();
        //         _fireParentNode.AddChild(smallFire);
        //         smallFire.GlobalPosition = (Vector2)v;
        //     }
        // }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
