using Godot;
using System;

public class Shotgun : Weapon
{
    
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.fire_range = 10;
        this.reload_rate = 1;
        this.clip_size = 2;
        this.fire_rate = 1;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
