using Godot;
using System;

public class Player : KinematicBody
{
	public int speed = 10;
	public int acceleration = 5;
	public float gravity = -09.8f;
	public int jump_power = 8;
	public double mouse_sensitivity = 0.3;
	public float current_y_velocity = 1f; 
	public int camera_x_rotation = 0;
	public Spatial head;
	public Camera camera;
	public Vector3 startDirection;
	public Vector3 direction;
	public Vector3 velocity;
	public Basis head_basis;


	public void ProcessInput(float delta)
	{
		head = this.GetNode<Godot.Spatial>("Head");
		camera = head.GetNode<Godot.Camera>("Camera");
		head_basis = head.GlobalTransform.basis;

		direction = startDirection;

		if (Input.IsActionPressed("move_forward"))
		{
			direction -= head_basis.z;
		}
		if (Input.IsActionPressed("move_backward"))
		{
			direction += head_basis.z;
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction -= head_basis.x;
		}
		if (Input.IsActionPressed("move_right"))
		{
			direction += head_basis.x;
		}

		direction = direction.Normalized();

		velocity = velocity.LinearInterpolate(direction * speed, acceleration * delta);

		velocity = MoveAndSlide(velocity, Vector3.Up);

		if (Input.IsActionJustPressed("ui_cancel"))
		{
			if (Input.GetMouseMode() == Input.MouseMode.Visible)
			{
				Input.SetMouseMode(Input.MouseMode.Captured);
			}
			else
			{
				Input.SetMouseMode(Input.MouseMode.Visible);
			}
		}

		if (!IsOnFloor())
		{
			current_y_velocity = current_y_velocity + (delta * gravity);
			//GD.Print(current_y_velocity, " and ", velocity.y);
			velocity.y = current_y_velocity;
		}
		else
		{
			//GD.Print("On floor");
		}
	}
	public void ProcessMovement()
	{
		direction.y = 0;

	}

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
			
			Mathf.Clamp(camera.RotateX((Convert.ToSingle((-x_delta) * (Math.PI / 180)))), -90, 90);
			camera_x_rotation = Mathf.Clamp(camera_x_rotation + (Convert.ToInt16(x_delta)), 90, -90);
			GD.Print(camera_x_rotation);
			
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		
	}
 	public override void _PhysicsProcess(float delta)
 	{
		
		ProcessInput(delta);
		

		

		

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			current_y_velocity = jump_power;
		}

		
	}
}
