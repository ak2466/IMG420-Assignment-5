using Godot;

/// <summary>
/// A basic collectible item.  When the player enters its Area2D, it spawns an
/// optional particle effect and then removes itself from the scene.  Attach this
/// script to an Area2D node with a CollisionShape2D and, optionally, assign a
/// Particles2D scene to the ParticlesScene property.
/// </summary>
public abstract partial class Pickup : Area2D
{
	/// <summary>
	/// A PackedScene pointing to a Particles2D node that will be instantiated when the pickup is collected.
	/// </summary>
	[Export]
	public PackedScene ParticlesScene;
	
	[Signal]
	public delegate void ItemCollectedEventHandler();

	public override void _Ready()
	{
		// Connect the body_entered signal to detect when the player touches the pickup
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	private void OnBodyEntered(Node2D body)
	{
		// Only react to the Player (customise this check as needed)
		if (body is Player)
		{
			// Call an abstract interaction method that the child class MUST define
			Interact(body);
			GD.Print("Generic interaction occurred (Pickup.cs)");
			
		}
	}
	
	protected abstract void Interact(Node2D body);
}
