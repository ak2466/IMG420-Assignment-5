using Godot;
using System;

public partial class DeathZone : Area2D
{
	public override void _Ready()
	{
		// Connect the body_entered signal to detect when the player touches the pickup
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}
	
	private void OnBodyEntered(Node2D body)
	{
		// Only react to the Player (customise this check as needed)
		if (body is Player player)
		{
			
			// Call an abstract interaction method that the child class MUST define
			GD.Print("Death zone crossed");
			player.Die();
			
		}
	}
}
