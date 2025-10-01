using Godot;
using System;


public partial class Main : Node2D
{
	private Random random = new Random();
	// 保存分数
	public int Score = 0;

	private Player player;
	private Label scoreLabel;
	public event Action ScoreChanged;
	// 生成子弹的预制体
	[Export] public PackedScene BulletScene;
	[Export] public PackedScene EnemyScene;
	public override void _Ready()
	{
		player=GetTree().Root.GetNode<Player>("Main/Player");
		scoreLabel = GetNode<Label>("CanvasLayer/Label");
		UpdateScoreLabel();
		GD.Print("Main Loaded");
	}

	private void SpawnEnemy()
	{


		// 实例化敌人
		Enemy enemy = EnemyScene.Instantiate<Enemy>();

		// 设置生成位置（随机环绕玩家）
		Vector2 spawnPos;
		if (player != null)
		{
			float angle = (float)(random.NextDouble() * Math.PI * 2);
			float distance = 300f;
			spawnPos = player.GlobalPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
		}
		else
		{
			// 没有玩家就随机在原点附近
			spawnPos = new Vector2((float)random.Next(-300, 300), (float)random.Next(-300, 300));
		}

		enemy.GlobalPosition = spawnPos;

		AddChild(enemy);
	}
	public override void _Process(double delta)
	{
		// 检查当前敌人数
		int enemyCount = GetTree().GetNodesInGroup("enemy").Count;
		GD.Print(enemyCount);
		if (enemyCount == 0)
		{
			int Num = random.Next(1, 5);
			for (int i = 0; i < Num; i++)
			{
				SpawnEnemy();
			}
		}
	}
	/// <summary>
	/// 增加分数
	/// </summary>
	public void AddScore(int value)
	{
		Score += value;
		GD.Print("Score: " + Score);
		UpdateScoreLabel();
	}

	/// <summary>
	/// 统一生成子弹
	/// </summary>
	public Bullet SpawnBullet(Vector2 position, Vector2 direction, Faction faction)
	{
		if (BulletScene == null) return null;

		var bullet = BulletScene.Instantiate<Bullet>();
		AddChild(bullet);
		bullet.GlobalPosition = position;
		bullet.Direction = direction.Normalized();
		bullet.ShooterFaction = faction;
		return bullet;
	}

	/// <summary>
	/// 重新开始关卡
	/// </summary>
	public void RestartGame()
	{
		GetTree().ReloadCurrentScene();
	}

	//更新分数显示

	private void UpdateScoreLabel()
	{
		scoreLabel.Text = $"Score: {Score}";
	}
}
