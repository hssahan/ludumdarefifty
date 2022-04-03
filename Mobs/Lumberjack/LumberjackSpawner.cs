using Godot;
using System;

public class LumberjackSpawner : Node2D
{
    [Export] public NodePath TimerNodePath;
    private readonly PackedScene _lumberjack = (PackedScene)ResourceLoader.Load("res://Mobs/Lumberjack/Lumberjack.tscn");
    private Timer _timer;
    private Lumberjack _lastLumberjack;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _timer = GetNode<Timer>(TimerNodePath);
        _timer.Connect("timeout", this, nameof(SpawnNewLumberjack));
        _timer.Start(2);
    }

    private void SpawnNewLumberjack()
    {
        _timer.Start(5);
        if(Godot.Object.IsInstanceValid(_lastLumberjack))
        {
            return;
        }
        var newLumberJack = (Lumberjack)_lumberjack.Instance();
        AddChild(newLumberJack);
        newLumberJack.GlobalPosition = GlobalPosition;
        _lastLumberjack = newLumberJack;
    }

}
