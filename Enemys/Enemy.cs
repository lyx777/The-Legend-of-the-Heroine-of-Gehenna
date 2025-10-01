using Godot;
using System;

public partial class Enemy : CombatActor
{
	private Player player;
	
	private float Speed=120f;
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("Main/Player");
		// 初始化玩家参数（直接写在代码里）
		MaxHealth = 5;
		BulletSpeed = 500f;
		ShootCD = 2f;

		// 指定子弹预制体（路径根据你项目实际修改）
		BulletScene = GD.Load<PackedScene>("res://Bullets/Bullet.tscn");

		base._Ready();
	}
	public override void _Process(double delta)
	{
		base._Process(delta);


		Shoot((player.GlobalPosition - GlobalPosition).Normalized(), Faction.Enemy);
		Vector2 dir = (player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = dir * Speed;
		MoveAndSlide();
	}

	protected override void Die()
	{
		(GetTree().CurrentScene as Main).AddScore(10);
		GD.Print("Enemy down!");
		base.Die();
	}
}
