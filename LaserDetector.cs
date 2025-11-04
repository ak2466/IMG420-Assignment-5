using Godot;

[Tool]
public partial class LaserDetector : Node2D
{
	private float _laserLength = 500f;
	
	[Export] 
	public float LaserLength
	{
		get => _laserLength;
		set
		{
			_laserLength = value;
			SetupVisuals();
			
		}
	}
	
	private float _laserWidth = 10f;
	[Export] 
	public float LaserWidth 
	{
		get => _laserWidth;
		set 
		{
			_laserWidth = value;
			// When the value changes in the editor, this setter is called
			// We call a method to update the visual property
			SetupVisuals();
		}
	}
	
	private Color _laserColorNormal = Colors.Green;
	[Export] public Color LaserColorNormal
	{
		get => _laserColorNormal;
		set
		{
			_laserColorNormal = value;
			SetupVisuals();
		}
	}
	[Export] public Color LaserColorAlert = Colors.Red;
	[Export] public NodePath PlayerPath;
	[Export] public float AlarmTimerWaitTime = 3.0f;
	
	[Export] public float MinOpacity = 0.5f;
	[Export] public float MaxOpacity = 1.0f;
	[Export] public float PulseSpeed = 5.0f;
	
	[Signal]
	public delegate void AlarmTriggeredEventHandler(Node2D player);
	
	private RayCast2D _rayCast;
	private Line2D _laserBeam;
	private Node2D _player;

	private bool _isAlarmActive = false;
	private Timer _alarmTimer;
	
	
	public override void _Ready()
	{
		SetupRaycast();
		SetupVisuals();
		SetupTimer();

		// TODO: Get player reference
		_player = GetTree().GetNodesInGroup("Player")[0] as Node2D;
		// TODO: Setup alarm timer
	}
	private void SetupRaycast()
	{
		// TODO: Create and configure RayCast2D
		// Hint: _rayCast = new RayCast2D();
		_rayCast = new RayCast2D();
		AddChild(_rayCast);

		// TODO: Set collision mask to detect player
		_rayCast.SetCollisionMaskValue(1, true);
	}
	private void SetupVisuals()
	{
		GD.Print("SETUP VISUALS CALLED");
		
		// Make sure this only runs once
		// 1. Check if the existing Line2D is in the scene tree.
		if (_laserBeam != null && IsInsideTree())
		{
			// 2. Remove the old Line2D node from the scene tree.
			// This is the "lazy" part! It cleans up the old visual.
			RemoveChild(_laserBeam);
			
			// 3. Mark the node for deletion (safety net)
			_laserBeam.QueueFree();
			
			// 4. Set the reference to null so SetupVisuals knows to create a new one.
			_laserBeam = null;
		}
		
		if (_laserBeam == null)
		{
			_laserBeam = new Line2D();
			
			if (IsInsideTree())
			{
				AddChild(_laserBeam);
			}
		}

		// TODO: Set width and color
		_laserBeam.DefaultColor = LaserColorNormal;
		_laserBeam.Width = _laserWidth;
		
		GD.Print($"SETUP VISUALS: LASER WIDTH {_laserBeam.Width}");
		
		_laserBeam.ClearPoints();

		// TODO: Add points for the line
		_laserBeam.AddPoint(Vector2.Zero);
		_laserBeam.AddPoint(new Vector2(LaserLength, 0));
	}

	private void SetupTimer()
	{
		
		// Make timer
		_alarmTimer = new Timer();
		
		// Set wait time of timer
		_alarmTimer.WaitTime = AlarmTimerWaitTime;
		_alarmTimer.OneShot = true;
		
		AddChild(_alarmTimer);
		
		_alarmTimer.Timeout += ResetAlarm;
	}
		
	public override void _PhysicsProcess(double delta)
	{
		
		//Set Target Position
		_rayCast.TargetPosition = Vector2.Right * LaserLength;

		// TODO: Force raycast update
		_rayCast.ForceRaycastUpdate();
		
		// TODO: Check if raycast is colliding
		Vector2? collisionPoint = null;

		if (_rayCast.IsColliding())
		{
			// TODO: Get collision point
			collisionPoint = _rayCast.GetCollisionPoint();
			
			// TODO: Check if hit object is player
			Node2D collisionObject = _rayCast.GetCollider() as Node2D;
			
			if (collisionObject == _player)
			{
				// TODO: Trigger alarm if player detected
				TriggerAlarm();
			}
			
		}
		
		// TODO: Update laser beam visualization
		UpdateLaserBeam(collisionPoint);


	}
	private void UpdateLaserBeam(Vector2? collisionPoint)
	{
		
		Vector2 endpoint;
		
		// TODO: Update Line2D points based on raycast
		if (collisionPoint.HasValue)
		{
			
			endpoint = ToLocal(collisionPoint.Value);

		}
		else
		{
			// Show full length if no collision
			endpoint = new Vector2(LaserLength, 0);
			
		}
		
		// Show up to collision point if hitting something
		_laserBeam.SetPointPosition(1, endpoint);
	}

	private void Pulse()
	{	
		// Get time
		float time = ((float)Time.GetTicksMsec()) * PulseSpeed / 1000f;
		float sinValue = Mathf.Sin(time);
		
		// Map to 0 - 1 range
		float normalizedSin = Mathf.Remap(sinValue, -1.0f, 1.0f, 0.0f, 1.0f);
		
		// Linearly interpolate between min and max
		float opacityValue = Mathf.Lerp(MinOpacity, MaxOpacity, normalizedSin);
		
		_laserBeam.DefaultColor = new Color(_laserBeam.DefaultColor, opacityValue);
	}
	private void TriggerAlarm()
	{
		// TODO: Change laser color
		_laserBeam.DefaultColor = LaserColorAlert;
		
		// TODO: Emit signal or call alarm function
		EmitSignal(SignalName.AlarmTriggered, _player);
		_alarmTimer.Start();
		
		float AlarmTimerTimeLeft = (float)_alarmTimer.TimeLeft;
		
		// TODO: Add visual feedback (flashing, particles, etc.)
		// Pulse
		Pulse();
		
		// TODO: Add audio feedback (optional)
		// No
		
	}
	private void ResetAlarm()
	{
		// TODO: Reset laser to normal color
		_laserBeam.DefaultColor = LaserColorNormal;
		
		// TODO: Reset alarm state
	}
}
