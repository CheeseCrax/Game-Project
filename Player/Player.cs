using Godot;
using System;

public class Player : KinematicBody
{
	
	public int speed = 10;
	public int acceleration = 5;
	public float gravity = 0.98;
	public int jump_power = 30;
	
	
	
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var head = this.GetNode<Godot.Spatial>("Head");
		var camera = this.GetNode<Godot.Camera>("Camera");
	}

	public void _Physics_Process(float delta)
	{
		var head_basis = head.get_global_transform().basis;
		var direction;
		if (Input.IsActionJustPressed("move_forward"))
		{
			direction -= head_basis.z;
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 	public override void _Process(float delta)
 	{
		 GD.Print("HI");
	}
}
