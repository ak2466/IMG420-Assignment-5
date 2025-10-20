using Godot;
using System;

public partial class Lock : Pickup
{
	
	private bool _isUnlocked = false;
	
	protected override void Interact(Node2D body)
	{
		
		
		
		// Check if all keys collected
		if (GlobalManager.Instance.NumCollectedKeys == GlobalManager.Instance.NumTotalKeys)
		{
			// Trigger animation
			TriggerParticles();
			
			// Free from queue
			QueueFree();
			
		}
	}
	
	private void TriggerParticles()
	{
		// Spawn particle effect at pickup position
		if (ParticlesScene == null)
		{
			GD.Print("ParticlesScene is NULL, please assign a Particles Scene in the editor.");
			return;
		}
		
		var particles = ParticlesScene.Instantiate() as GpuParticles2D;
		
		if (particles == null)
		{
			GD.Print("Failed to instantiate particles from ParticlesScene");
			return;
		}
		
		particles.GlobalPosition = GlobalPosition;
		GetParent().AddChild(particles);
		
		particles.Emitting = true;
		
		// Remove the pickup from the scene
		particles.Finished += () => particles.QueueFree();
	}
}
