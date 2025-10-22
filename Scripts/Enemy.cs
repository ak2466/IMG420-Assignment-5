using Godot;

/// <summary>
/// A simple enemy that uses a NavigationAgent2D to follow the player.  To use this
/// script, create a CharacterBody2D with a NavigationAgent2D child.  In the
/// inspector, set the "TargetPath" export to point to the Player node.  Ensure
/// your TileSet has a NavigationLayer painted on walkable tiles and that your
/// TileMap has a NavigationRegion2D so the agent can compute paths.  The enemy
/// will continuously update its target and move along the computed path.
/// </summary>
public partial class Enemy : CharacterBody2D
{
	[Export]
	public float Speed = 120f;

	/// <summary>
	/// Exposed NodePath to assign the target (e.g. Player) in the editor.
	/// </summary>
	[Export]
	public NodePath TargetPath;

	private NavigationAgent2D _navAgent;
	private Area2D _area2d;
	private Node2D _target;
	private AnimatedSprite2D _anim;

	public override void _Ready()
	{
		_navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

		if (TargetPath != null)
		{
			_target = GetNode<Node2D>(TargetPath);
		}
		
		// Get the Area2D and link the body entered signa
		_area2d = GetNode<Area2D>("Hitbox");
		_area2d.BodyEntered += OnBodyEntered;

		// Configure navigation agent properties if needed (e.g., max speed)
		// _navAgent.MaxSpeed = Speed;
		
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	public void OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			// Call an abstract interaction method that the child class MUST define
			GD.Print("Enemy touched");
			player.Die();
			
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_target == null || _navAgent == null)
			return;

		// Update the navigation target each frame to follow the player's current position
		_navAgent.TargetPosition = _target.GlobalPosition;

		// Retrieve the next point along the computed path
		Vector2 nextPoint = _navAgent.GetNextPathPosition();


		// Compute direction towards the next point
		Vector2 rawDirection = (nextPoint - GlobalPosition).Normalized();
		Vector2 desiredVelocity = rawDirection * Speed;
		
		_navAgent.SetVelocity(desiredVelocity);

		// Move towards the target
		Velocity = desiredVelocity;
		
		if (_navAgent.IsNavigationFinished())
		{
			Velocity = Vector2.Zero;
			_anim.Play("idle");
			return;
		}
		
		MoveAndSlide();
		
				// Flip and play animations based on movement
		if (_anim != null)
		{
			if (Mathf.Abs(Velocity.X) > 0.01f)
			{
				_anim.FlipH = Velocity.X < 0; 
				_anim.Play("walk");
			}
			else
			{
				_anim.Play("idle");
			}
		}
	}
	
	
}
