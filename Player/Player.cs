using Godot;
using System;

public class Player : KinematicBody
{
	
	public int speed = 10;
	public int acceleration = 5;
	public double gravity = 0.98;
	public int jump_power = 30;
	public double mouse_sensitivity = 0.3;
	public Spatial head;
	public Camera camera;
	public Vector3 startDirection;
	public Vector3 direction;
	public Vector3 velocity;
	public int camera_x_rotation = 0;
	public Basis head_basis;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.SetMouseMode(Input.MouseMode.Captured);
		head = this.GetNode<Godot.Spatial>("Head");
		camera = head.GetNode<Godot.Camera>("Camera");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			head.RotateY(Convert.ToSingle((-eventMouseMotion.Relative.x * mouse_sensitivity) * (Math.PI / 180)));

			var x_delta = eventMouseMotion.Relative.y * mouse_sensitivity;
			if (camera_x_rotation + x_delta > -90 && camera_x_rotation + x_delta < 90) 
			{
				camera.RotateX(Convert.ToSingle((-x_delta) * (Math.PI / 180)));
				camera_x_rotation += Convert.ToInt16(x_delta);
			}
			
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			Input.SetMouseMode(Input.MouseMode.Visible);
		}
	}
 	public override void _PhysicsProcess(float delta)
 	{

		head = this.GetNode<Godot.Spatial>("Head");
		camera = head.GetNode<Godot.Camera>("Camera");
		head_basis = head.GlobalTransform.basis;

		direction = startDirection;
		if (Input.IsActionPressed("move_forward"))
		{
			direction -= head_basis.z;
		}else if (Input.IsActionPressed("move_backward"))
		{
			direction += head_basis.z;
		}else if (Input.IsActionPressed("move_left"))
		{
			direction -= head_basis.x;
		}else if (Input.IsActionPressed("move_right"))
		{
			direction += head_basis.x;
		}

		direction = direction.Normalized();

		velocity = velocity.LinearInterpolate(direction * speed, acceleration * delta);
		velocity.y -= Convert.ToSingle(gravity);

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.y += Convert.ToSingle(jump_power);
		}

		velocity = MoveAndSlide(velocity, Vector3.Up, false, 4, Convert.ToSingle(0.785385), false);
	}
}
