using Godot;
using System;
using System.Numerics;
using System.Security.Cryptography;

public partial class Player : CombatActor
{
	public float moveSpeed = 250f;
	public int Mag = 3; // 弹夹容量

	public float ReloadTime = 1.5f;
	public override void _Ready()
	{
		// 初始化玩家参数
		MaxHealth = 15;
		BulletSpeed = 500f;
		ShootCD = 0.3f;
		BulletScene = GD.Load<PackedScene>("res://Bullets/Bullet.tscn");

		base._Ready();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if(ReloadTime>=0)ReloadTime -= (float)delta;

		if (ReloadTime <= 0 && Mag == 0)
		{
			Mag = 3;
		}

		Godot.Vector2 inputDir = Godot.Vector2.Zero;

		if (Input.IsActionPressed("right")) inputDir.X += 1;
		if (Input.IsActionPressed("left")) inputDir.X -= 1;
		if (Input.IsActionPressed("down")) inputDir.Y += 1;
		if (Input.IsActionPressed("up")) inputDir.Y -= 1;

		Velocity = inputDir.Normalized() * 200f;
		MoveAndSlide();


		if (Input.IsActionPressed("shoot")&&Mag>0)
		{
			Mag--;
			Shoot((GetGlobalMousePosition() - GlobalPosition).Normalized(), Faction.Player); 
		}
		if (Mag == 0&&ReloadTime<=0)
		{
			ReloadTime = 1.5f;
		}
	}

	protected override void Die()
	{
		GD.Print("Player Dead!");
		base.Die();
		(GetTree().Root.GetNode<Label>("Main/CanvasLayer/EndGame")).Visible = true;
	}
}
