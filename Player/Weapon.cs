using Godot;
using System;

public class Weapon : Node
{
    [Export] public double fire_rate = 0.5;
    [Export] public int clip_size = 5;
    [Export] public double reload_rate = 1;
    [Export] public int fire_range = 100;
    
    public int current_ammo;
    public bool can_fire = true;
    public bool reloading = false;
    public RayCast raycast;
    public Label ammo_label;
    public Node root;

    public void Check_Collision()
    {
        if (raycast.IsColliding())
        {
            var obj = raycast.GetCollider() as Node;
            if (obj != null && obj.IsInGroup("Enemies"))
            {
                obj.QueueFree();
                GD.Print("Killed " , obj.Name);
            }
        }
    }

    public async void Fire()
    {
        if (can_fire)
        {
                if (current_ammo > 0 &! reloading)
                {
                    can_fire = false;
                    current_ammo = current_ammo - 1;
                    Check_Collision();
                    GD.Print("brother in christ");
                    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(fire_rate));
                    can_fire = true;
                }
                else if (!reloading)
                {
                    Reload();
                }
        }
    }

    public async void Reload()
    {
        reloading = true;
        GD.Print("Reloading");
        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(reload_rate));
        current_ammo = clip_size;
        reloading = false;
        GD.Print("Reload done");
    }

    public override void _Process(float delta)
    {
        ammo_label.Text = $"{current_ammo} / {clip_size}";

        if (Input.IsActionJustPressed("primary_fire"))
        {
            Fire();
        }
        
        if (Input.IsActionJustPressed("reload"))
        {
            Reload();
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        root = this.GetTree().Root;
        var player = this.GetParent<Godot.Spatial>();
        raycast = player.GetNode<Godot.RayCast>("./Head/Camera/RayCast");
        raycast.CastTo = new Vector3(0, 0, -fire_range);
        ammo_label = root.GetNode<Godot.Label>("./World/UI/Label");
        current_ammo = clip_size;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
